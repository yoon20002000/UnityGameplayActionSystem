using UnityEngine;
[RequireComponent(typeof(ActionSystem))]
[RequireComponent(typeof(HealthSystem))]
public class Character : MonoBehaviour
{
    public Transform GetShotTransform()
    {
        return shotTransform;

    }
    public virtual Ray GetShotRay()
    {
        return default;
    }
    public ActionSystem GetActionSystem()
    {
        return actionSystem;
    }
    
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
    private void Awake()
    {
        if(healthSystem == null)
        {
            healthSystem = GetComponent<HealthSystem>();
        }
            
        if(actionSystem == null)
        {
            actionSystem = GetComponent<ActionSystem>();
        }
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
    [SerializeField]
    protected ActionSystem actionSystem;
    [SerializeField]
    protected HealthSystem healthSystem;

    [SerializeField]
    protected Transform shotTransform;
}
