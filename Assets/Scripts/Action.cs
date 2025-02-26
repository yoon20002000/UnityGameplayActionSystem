using UnityEngine;

public class Action : MonoBehaviour
{
    public void Initialize(ActionSystem InActionSystem)
    {
        actionSystem = InActionSystem;
    }

    public ActionSystem GetActionSystem()
    {
        return actionSystem;
    }

    public GameplayTags GetActivationTag()
    {
        return activationTag;
    }
    public bool GetAtuoStart()
    {
        return bAutoStart;
    }
    protected void SetAtuoStart(bool bInAutoStart)
    {
        bAutoStart = bInAutoStart;
    }
    public bool IsRunning()
    {
        return bIsRunning;
    }

    public virtual bool IsCanStart(GameObject instigator)
    {
        if(IsRunning() == true)
        {
            return false;
        }
        if(actionSystem.ActiveTagHasAny(blockedTags))
        {
            return false;
        }

        return true;
    }

    public virtual void StartAction(GameObject inInstigator)
    {
        actionSystem.SetActiveTags(grantsTags);

        bIsRunning = true;
        instigator = inInstigator;

        actionSystem.OnActionStated.Invoke(actionSystem, this);
    }

    public virtual void StopAction(GameObject inInstigator)
    {
        actionSystem.UnSetActiveTags(grantsTags);

        bIsRunning = false;
        instigator = inInstigator;

        actionSystem.OnActionStoped.Invoke(actionSystem, this);
    }

    protected ActionSystem actionSystem;

    // editor에서 하나만 선택하게 해야 됨.
    [SerializeField]
    protected GameplayTags activationTag;

    [SerializeField]
    protected GameplayTags grantsTags;
    [SerializeField]
    protected GameplayTags blockedTags;

    protected bool bIsRunning = false;
    protected GameObject instigator = null;

    protected float timeTarted;

    protected bool bAutoStart = false;
}
