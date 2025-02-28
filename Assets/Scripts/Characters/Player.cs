using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    private void Awake()
    {
        
    }
    private void OnEnable()
    {        
        add_perfored(ref attackInput, ATTACK_ACTION_INPUT_NAME, attackInput_Performed);
        add_Started(ref defenceInput, DEFENCE_ACTION_INPUT_NAME, defence_Started);
        add_Canceled(ref defenceInput, DEFENCE_ACTION_INPUT_NAME, defence_Canceld);
        add_Started(ref skill1Input, SKILL1_ACTION_INPUT_NAME, skill1_Started);
        add_Started(ref skill2Input, SKILL2_ACTION_INPUT_NAME, skill2_Started);
        add_Started(ref dashInput, DASH_ACTION_INPUT_NAME, dash_Started);
        add_Started(ref aimInput, AIM_ACTION_INPUT_NAME, aim_Started);
        add_Canceled(ref aimInput, AIM_ACTION_INPUT_NAME, aim_Canceld);

        add_perfored(ref lookInput, LOOK_ACTION_INPUT_NAME, look_Performed);

        add_perfored(ref moveInput, MOVE_ACTION_INPUT_NAME, moveInput_Performed);
        add_Canceled(ref moveInput, MOVE_ACTION_INPUT_NAME, moveInput_Canceld);

        bindActionChanged();
    }

   

    private void FixedUpdate()
    {
        move();
    }

    private void OnDisable()
    {
        attackInput.Disable();
        unBindActionChanged();
    }
    private void OnDestroy()
    {
        unBindActionChanged();
    }

    private void attackInput_Performed(InputAction.CallbackContext obj)
    {
        actionSystem.StartActionByTag(this, EGameplayTags.Action_Attack);
    }
    private void add_perfored(ref InputAction inputAction, string actionName,
        System.Action<InputAction.CallbackContext> performed = null, bool bIsAutoEnable = true)
    {
        add_inputActionCallback(ref inputAction, actionName, null, performed, null, bIsAutoEnable);
    }
    private void add_Started(ref InputAction inputAction, string actionName, System.Action<InputAction.CallbackContext> started = null, bool bIsAutoEnable = true)
    {
        add_inputActionCallback(ref inputAction, actionName, started, null, null, bIsAutoEnable);
    }
    private void add_Canceled(ref InputAction inputAction, string actionName, System.Action<InputAction.CallbackContext> canceled = null, bool bIsAutoEnable = true)
    {
        add_inputActionCallback(ref inputAction, actionName, null, null, canceled, bIsAutoEnable);
    }
    private void add_inputActionCallback(ref InputAction inputAction, string actionName,
        System.Action<InputAction.CallbackContext> started = null,
        System.Action<InputAction.CallbackContext> performed = null,
        System.Action<InputAction.CallbackContext> canceled = null,
        bool bIsAutoEnable = true)
    {
        inputAction = inputActionAsset.FindAction(actionName);

        if (inputAction == null)
        {
            return;
        }

        if(started != null)
        {
            inputAction.started += started;
        }

        if (performed != null)
        {
            inputAction.performed += performed;
        }

        if(canceled != null)
        {
            inputAction.canceled += canceled;
        }

        if (bIsAutoEnable == true)
        {
            inputAction.Enable();
        }
    }
    private void defence_Started(InputAction.CallbackContext context)
    {
        actionSystem.StartActionByTag(this, EGameplayTags.Action_Deffence);
    }
    private void defence_Canceld(InputAction.CallbackContext context)
    {
        actionSystem.StopActionByTag(this, EGameplayTags.Action_Deffence);
    }
    private void skill1_Started(InputAction.CallbackContext context)
    {
        actionSystem.StartActionByTag(this, EGameplayTags.Action_Skill1);
    }
    private void skill2_Started(InputAction.CallbackContext context)
    {
        actionSystem.StartActionByTag(this, EGameplayTags.Action_Skill2);
    }
    private void dash_Started(InputAction.CallbackContext context)
    {
        actionSystem.StartActionByTag(this, EGameplayTags.Action_Dash);
    }
    private void aim_Started(InputAction.CallbackContext context)
    {
        mainCamera.fieldOfView = aimFOV;
        actionSystem.StartActionByTag(this, EGameplayTags.Action_Aim);
    }
    private void aim_Canceld(InputAction.CallbackContext context)
    {
        mainCamera.fieldOfView = normalFOV;
        actionSystem.StopActionByTag(this, EGameplayTags.Action_Aim);
    }

    private void look_Performed(InputAction.CallbackContext context)
    {
        //Vector2 lookInput = context.ReadValue<Vector2>();

        //float mouseX = lookInput.x * mouseSensitivity;
        //float mouseY = lookInput.y * mouseSensitivity;

        //verticalRotation -= mouseY;
        //verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        //mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        //transform.Rotate(Vector3.up * mouseX);
    }
    private void moveInput_Performed(InputAction.CallbackContext context)
    {
        moveInputValue = context.ReadValue<Vector2>();
        actionSystem.SetActiveTags(EGameplayTags.Status_Moving);
    }

    private void moveInput_Canceld(InputAction.CallbackContext context)
    {
        moveInputValue = Vector2.zero;
        actionSystem.UnSetActiveTags(EGameplayTags.Status_Moving);
    }
    private void move()
    {
        Vector3 moveDirection = new Vector3( moveInputValue.x , 0, moveInputValue.y);
        moveDirection.y = 0;
        rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
        this.transform.forward = moveDirection;
    }
    private void aiming()
    {

    }

    [SerializeField]
    private InputActionAsset inputActionAsset;

    private readonly static string ATTACK_ACTION_INPUT_NAME = "Attack";
    private readonly static string DEFENCE_ACTION_INPUT_NAME = "Defence";
    private readonly static string SKILL1_ACTION_INPUT_NAME = "Skill1";
    private readonly static string SKILL2_ACTION_INPUT_NAME = "Skill2";
    private readonly static string DASH_ACTION_INPUT_NAME = "Dash";
    private readonly static string AIM_ACTION_INPUT_NAME = "Aim";
    private readonly static string MOVE_ACTION_INPUT_NAME = "Move";
    private readonly static string LOOK_ACTION_INPUT_NAME = "Look";

    private InputAction attackInput;
    private InputAction defenceInput;
    private InputAction skill1Input;
    private InputAction skill2Input;
    private InputAction dashInput;
    private InputAction aimInput;
    private InputAction moveInput;
    private InputAction lookInput;

    [Header("Movement & Rigidbody")]
    private Vector2 moveInputValue;
    [SerializeField]
    private float moveSpeed = 50.0f;
    [SerializeField]
    private Rigidbody rb;

    [Header("Camera")]
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private float aimFOV = 30;
    [SerializeField]
    private float normalFOV = 60;
    [SerializeField]
    private float mouseSensitivity = 2f;
    public float minVerticalAngle = -45f;
    public float maxVerticalAngle = 75f;

    private float verticalRotation = 0f;
}
