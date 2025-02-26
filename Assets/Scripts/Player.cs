using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ActionSystem actionSystem;

    [SerializeField]
    private InputActionAsset inputActionAsset;

    private InputAction attackInput;
    private void OnEnable()
    {
        attackInput = inputActionAsset.FindAction("Attack");
        attackInput.performed += attackInput_performed;
        attackInput.Enable();
    }
    private void OnDisable()
    {
        attackInput.Disable();
    }

    private void attackInput_performed(InputAction.CallbackContext obj)
    {
        actionSystem.StartActionByTag(this.gameObject, GameplayTags.Action_Attack);
    }
}
