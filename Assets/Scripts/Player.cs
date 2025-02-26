using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    ActionSystem actionSystem;

    [SerializeField]
    InputActionAsset inputActionAsset;

    [SerializeField]
    InputAction attackInput;
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
        Debug.Log("Attack Input");
    }
}
