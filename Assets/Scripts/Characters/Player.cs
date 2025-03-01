using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class Player : Character
{
    public override Ray GetShotRay()
    {
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        Camera curCamera = brain.OutputCamera;
        
        if(curCamera != null)
        {
            
            return curCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        }
        else
        {
            Debug.LogError("Aim Camera is not setted!!!!!");
        }

        return default;
    }
    private void Awake()
    {
        if(uiCanvas.activeSelf == true)
        {
            uiCanvas.SetActive(false);
        }
        Assert.IsNotNull(cameraRig, "Aim Camera rig is not setted.");
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

        add_perfored(ref moveInput, MOVE_ACTION_INPUT_NAME, moveInput_Performed);
        add_Canceled(ref moveInput, MOVE_ACTION_INPUT_NAME, moveInput_Canceld);

        bindActionChanged();
    } 

    private void FixedUpdate()
    {
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
        uiCanvas.SetActive(true);
        actionSystem.StartActionByTag(this, EGameplayTags.Action_Aim);
    }
    private void aim_Canceld(InputAction.CallbackContext context)
    {
        uiCanvas.SetActive(false);
        actionSystem.StopActionByTag(this, EGameplayTags.Action_Aim);
    }
    private void moveInput_Performed(InputAction.CallbackContext context)
    {
        actionSystem.SetActiveTags(EGameplayTags.Status_Moving);
    }

    private void moveInput_Canceld(InputAction.CallbackContext context)
    {
        actionSystem.UnSetActiveTags(EGameplayTags.Status_Moving);
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

    private InputAction attackInput;
    private InputAction defenceInput;
    private InputAction skill1Input;
    private InputAction skill2Input;
    private InputAction dashInput;
    private InputAction aimInput;
    private InputAction moveInput;

    [SerializeField]
    private GameObject uiCanvas;
    [SerializeField]
    private AimCameraRig cameraRig;
}
