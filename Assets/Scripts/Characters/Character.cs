using UnityEngine;
[RequireComponent(typeof(ActionSystem))]
[RequireComponent(typeof(HealthSystem))]
public class Character : MonoBehaviour
{
    [SerializeField]
    protected ActionSystem actionSystem;
    public ActionSystem GetActionSystem()
    {
        return actionSystem;
    }
    [SerializeField]
    protected HealthSystem healthSystem;
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
    private void OnEnable()
    {
        bindActionChanged();
    }
    private void OnDisable()
    {
        unBindActionChanged();
    }
    private void OnDestroy()
    {
        unBindActionChanged();
    }
    protected virtual void bindActionChanged()
    {
        if (actionSystem != null)
        {
            actionSystem.OnActionStarted += onActionStarted;
            actionSystem.OnActionStoped += onActionStoped;
        }
    }
    protected virtual void unBindActionChanged()
    {
        if (actionSystem != null)
        {
            actionSystem.OnActionStarted -= onActionStarted;
            actionSystem.OnActionStoped -= onActionStoped;
        }
    }
    private void onActionStarted(ActionSystem system, Action action)
    {
        checkCharacterState();
    }
    private void onActionStoped(ActionSystem system, Action action)
    {
        checkCharacterState();
    }
    protected void checkCharacterState()
    {
        checkInvincibility();
    }
    private void checkInvincibility()
    {
        bool bIsInvincibility = actionSystem.ActiveTagHasAny(EGameplayTags.Status_Invincibility);
        healthSystem.SetInvincibility(bIsInvincibility);
    }
}
