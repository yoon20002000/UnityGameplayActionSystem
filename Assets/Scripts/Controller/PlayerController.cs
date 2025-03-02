using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
public abstract class PlayerControllerBase : MonoBehaviour, Unity.Cinemachine.IInputAxisOwner
{
    [Tooltip("Ground speed when walking")]
    public float Speed = 6f;

    public System.Action PreUpdate;
    public System.Action<Vector3, float> PostUpdate;
    public System.Action StartJump;
    public System.Action EndJump;

    [Header("Input Axes")]
    [Tooltip("X Axis movement.  Value is -1..1.  Controls the sideways movement")]
    public InputAxis MoveX = InputAxis.DefaultMomentary;

    [Tooltip("Z Axis movement.  Value is -1..1. Controls the forward movement")]
    public InputAxis MoveZ = InputAxis.DefaultMomentary;

    /// Report the available input axes to the input axis controller.
    /// We use the Input Axis Controller because it works with both the Input package
    /// and the Legacy input system.  This is sample code and we
    /// want it to work everywhere.
    void IInputAxisOwner.GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
    {
        axes.Add(new() { DrivenAxis = () => ref MoveX, Name = "Move X", Hint = IInputAxisOwner.AxisDescriptor.Hints.X });
        axes.Add(new() { DrivenAxis = () => ref MoveZ, Name = "Move Z", Hint = IInputAxisOwner.AxisDescriptor.Hints.Y });
    }

    public virtual void SetStrafeMode(bool b) { }
    public abstract bool IsMoving { get; }
}
public class PlayerController : PlayerControllerBase
{
    [Tooltip("Transition duration (in seconds) when the player changes velocity or rotation.")]
    public float Damping = 0.5f;

    [Tooltip("Makes the player strafe when moving sideways, otherwise it turns to face the direction of motion.")]
    public bool Strafe = false;

    public enum ForwardModes { Camera, Player, World };
    public enum UpModes { Player, World };

    [Tooltip("Reference frame for the input controls:\n"
        + "<b>Camera</b>: Input forward is camera forward direction.\n"
        + "<b>Player</b>: Input forward is Player's forward direction.\n"
        + "<b>World</b>: Input forward is World forward direction.")]
    public ForwardModes InputForward = ForwardModes.Camera;

    [Tooltip("Up direction for computing motion:\n"
        + "<b>Player</b>: Move in the Player's local XZ plane.\n"
        + "<b>World</b>: Move in global XZ plane.")]
    public UpModes UpMode = UpModes.World;

    [Tooltip("If non-null, take the input frame from this camera instead of Camera.main. Useful for split-screen games.")]
    public Camera CameraOverride;

    [Tooltip("Layers to include in ground detection via Raycasts.")]
    public LayerMask GroundLayers = 1;

    Vector3 m_CurrentVelocityXZ;
    Vector3 m_LastInput;

    // These are part of a strategy to combat input gimbal lock when controlling a player
    // that can move freely on surfaces that go upside-down relative to the camera.
    // This is only used in the specific situation where the character is upside-down relative to the input frame,
    // and the input directives become ambiguous.
    // If the camera and input frame are travelling along with the player, then these are not used.
    bool m_InTopHemisphere = true;
    float m_TimeInHemisphere = 100;
    Vector3 m_LastRawInput;
    Quaternion m_Upsidedown = Quaternion.AngleAxis(180, Vector3.left);

    public override void SetStrafeMode(bool b) => Strafe = b;
    public override bool IsMoving => m_LastInput.sqrMagnitude > 0.01f;
    public Camera Camera => CameraOverride == null ? Camera.main : CameraOverride;

    public bool IsGrounded() => GetDistanceFromGround(transform.position, UpDirection, 10) < 0.01f;

    private void OnEnable()
    {
    }

    void Update()
    {
        PreUpdate?.Invoke();

        // Get the reference frame for the input
        var rawInput = new Vector3(MoveX.Value, 0, MoveZ.Value);
        var inputFrame = GetInputFrame(Vector3.Dot(rawInput, m_LastRawInput) < 0.8f);
        m_LastRawInput = rawInput;

        // Read the input from the user and put it in the input frame
        m_LastInput = inputFrame * rawInput;
        if (m_LastInput.sqrMagnitude > 1)
            m_LastInput.Normalize();

        var desiredVelocity = m_LastInput * Speed;

        if (Vector3.Angle(m_CurrentVelocityXZ, desiredVelocity) < 100)
            m_CurrentVelocityXZ = Vector3.Slerp(
                m_CurrentVelocityXZ, desiredVelocity,
                Damper.Damp(1, Damping, Time.deltaTime));
        else
            m_CurrentVelocityXZ += Damper.Damp(
                desiredVelocity - m_CurrentVelocityXZ, Damping, Time.deltaTime);


        // Apply the position change
        ApplyMotion();

        // If not strafing, rotate the player to face movement direction
        if (!Strafe && m_CurrentVelocityXZ.sqrMagnitude > 0.001f)
        {
            var fwd = inputFrame * Vector3.forward;
            var qA = transform.rotation;
            var qB = Quaternion.LookRotation(
                (InputForward == ForwardModes.Player && Vector3.Dot(fwd, m_CurrentVelocityXZ) < 0)
                    ? -m_CurrentVelocityXZ : m_CurrentVelocityXZ, UpDirection);
            transform.rotation = Quaternion.Slerp(qA, qB, Damper.Damp(1, Damping, Time.deltaTime));
        }

        if (PostUpdate != null)
        {
            // Get local-space velocity
            var vel = Quaternion.Inverse(transform.rotation) * m_CurrentVelocityXZ;
            PostUpdate(vel, 1);
        }
    }

    Vector3 UpDirection => UpMode == UpModes.World ? Vector3.up : transform.up;

    // Get the reference frame for the input.  The idea is to map camera fwd/right
    // to the player's XZ plane.  There is some complexity here to avoid
    // gimbal lock when the player is tilted 180 degrees relative to the input frame.
    Quaternion GetInputFrame(bool inputDirectionChanged)
    {
        // Get the raw input frame, depending of forward mode setting
        var frame = Quaternion.identity;
        switch (InputForward)
        {
            case ForwardModes.Camera: frame = Camera.transform.rotation; break;
            case ForwardModes.Player: return transform.rotation;
            case ForwardModes.World: break;
        }

        // Map the raw input frame to something that makes sense as a direction for the player
        var playerUp = transform.up;
        var up = frame * Vector3.up;

        // Is the player in the top or bottom hemisphere?  This is needed to avoid gimbal lock,
        // but only when the player is upside-down relative to the input frame.
        const float BlendTime = 2f;
        m_TimeInHemisphere += Time.deltaTime;
        bool inTopHemisphere = Vector3.Dot(up, playerUp) >= 0;
        if (inTopHemisphere != m_InTopHemisphere)
        {
            m_InTopHemisphere = inTopHemisphere;
            m_TimeInHemisphere = Mathf.Max(0, BlendTime - m_TimeInHemisphere);
        }

        // If the player is untilted relative to the input frmae, then early-out with a simple LookRotation
        var axis = Vector3.Cross(up, playerUp);
        if (axis.sqrMagnitude < 0.001f && inTopHemisphere)
            return frame;

        // Player is tilted relative to input frame: tilt the input frame to match
        var angle = UnityVectorExtensions.SignedAngle(up, playerUp, axis);
        var frameA = Quaternion.AngleAxis(angle, axis) * frame;

        // If the player is tilted, then we need to get tricky to avoid gimbal-lock
        // when player is tilted 180 degrees.  There is no perfect solution for this,
        // we need to cheat it :/
        Quaternion frameB = frameA;
        if (!inTopHemisphere || m_TimeInHemisphere < BlendTime)
        {
            // Compute an alternative reference frame for the bottom hemisphere.
            // The two reference frames are incompatible where they meet, especially
            // when player up is pointing along the X axis of camera frame. 
            // There is no one reference frame that works for all player directions.
            frameB = frame * m_Upsidedown;
            var axisB = Vector3.Cross(frameB * Vector3.up, playerUp);
            if (axisB.sqrMagnitude > 0.001f)
                frameB = Quaternion.AngleAxis(180f - angle, axisB) * frameB;
        }
        // Blend timer force-expires when user changes input direction
        if (inputDirectionChanged)
            m_TimeInHemisphere = BlendTime;

        // If we have been long enough in one hemisphere, then we can just use its reference frame
        if (m_TimeInHemisphere >= BlendTime)
            return inTopHemisphere ? frameA : frameB;

        // Because frameA and frameB do not join seamlessly when player Up is along X axis,
        // we blend them over a time in order to avoid degenerate spinning.
        // This will produce weird movements occasionally, but it's the lesser of the evils.
        if (inTopHemisphere)
            return Quaternion.Slerp(frameB, frameA, m_TimeInHemisphere / BlendTime);
        return Quaternion.Slerp(frameA, frameB, m_TimeInHemisphere / BlendTime);
    }

    void ApplyMotion()
    {
        var pos = transform.position + m_CurrentVelocityXZ * Time.deltaTime;

        // Don't fall below ground
        var up = UpDirection;
        var altitude = GetDistanceFromGround(pos, up, 10);

        transform.position = pos;
    }

    float GetDistanceFromGround(Vector3 pos, Vector3 up, float max)
    {
        float kExtraHeight = 0; // start a little above the player in case it's moving down fast
        if (Physics.Raycast(pos + up * kExtraHeight, -up, out var hit,
                max + kExtraHeight, GroundLayers, QueryTriggerInteraction.Ignore))
            return hit.distance - kExtraHeight;
        return max + 1;
    }
}