using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    [SerializeField]
    private InputActionAsset inputActionAsset;

    private readonly static string ATTACK_ACTION_INPUT_NAME = "Attack";
    private readonly static string DEFENCE_ACTION_INPUT_NAME = "Defence";
    private readonly static string SKILL1_ACTION_INPUT_NAME = "Skill1";
    private readonly static string SKILL2_ACTION_INPUT_NAME = "Skill2";
    private readonly static string DASH_ACTION_INPUT_NAME = "Dash";
    private readonly static string AIM_ACTION_INPUT_NAME = "Aim";
    
    private InputAction attackInput;
    private InputAction defenceInput;
    private InputAction skill1Input;
    private InputAction skill2Input;
    private InputAction dashInput;
    private InputAction aimInput;
    
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
        bindActionChanged();
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
        actionSystem.StartActionByTag(this, EGameplayTags.Action_Aim);
    }
    private void aim_Canceld(InputAction.CallbackContext context)
    {
        actionSystem.StopActionByTag(this, EGameplayTags.Action_Aim);
    }
}
