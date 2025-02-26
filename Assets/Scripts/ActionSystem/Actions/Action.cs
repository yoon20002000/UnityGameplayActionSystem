using UnityEngine.Assertions;
using UnityEngine;

public class Action : MonoBehaviour
{
    public Action()
    {
        activationTag = GameplayTags.None_Action;
        grantsTags = GameplayTags.None_Action;
        blockedTags = GameplayTags.None_Action;
    }
    public virtual void DeepCopy(Action other)
    {
        Assert.IsNotNull(other, "You DeepCopy using original action resources");

        this.actionSystem = other.actionSystem;
        this.activationTag = other.activationTag;
        this.grantsTags = other.grantsTags;
        this.blockedTags = other.blockedTags;
        this.bIsRunning = other.bIsRunning;
        this.bAutoStart = other.bAutoStart;
        this.timeStarted = other.timeStarted;
        this.instigator = other.instigator;
    }
    public virtual void Initialize(ActionSystem InActionSystem, Action other = null)
    {
        actionSystem = InActionSystem;
        if(other != null)
        {
            this.activationTag = other.activationTag;
            this.grantsTags = other.grantsTags;
            this.blockedTags = other.blockedTags;
            this.bIsRunning = other.bIsRunning;
            this.bAutoStart = other.bAutoStart;
            this.timeStarted = other.timeStarted;
            this.instigator = other.instigator;
        }
    }

    public ActionSystem GetActionSystem()
    {
        return actionSystem;
    }

    public GameplayTags GetActivationTag()
    {
        return activationTag;
    }
    public GameplayTags GetGrantsTags()
    {
        return grantsTags;
    }
    public bool GetAutoStart()
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

    public virtual bool IsCanStart(Character inInstigator)
    {
        if (IsRunning() == true)
        {
            return false;
        }
        if (actionSystem.ActiveTagHasAny(blockedTags))
        {
            return false;
        }

        return true;
    }

    public virtual void StartAction(Character inInstigator)
    {
        Debug.LogFormat("Start Action : {0}, Instigator : {1}", activationTag.ToString(), inInstigator.name );
        actionSystem.SetActiveTags(grantsTags);

        bIsRunning = true;
        instigator = inInstigator;

        timeStarted = Time.time;

        actionSystem.OnActionStated?.Invoke(actionSystem, this);
    }

    public virtual void StopAction(Character inInstigator)
    {
        actionSystem.UnSetActiveTags(grantsTags);

        bIsRunning = false;
        instigator = inInstigator;

        actionSystem.OnActionStoped?.Invoke(actionSystem, this);
    }

    protected ActionSystem actionSystem = null;

    // editor에서 하나만 선택하게 해야 됨.
    [SerializeField]
    protected GameplayTags activationTag;

    [SerializeField]
    protected GameplayTags grantsTags;
    [SerializeField]
    protected GameplayTags blockedTags;

    [SerializeField]
    protected bool bIsRunning = false;
    protected Character instigator = null;

    protected float timeStarted;

    [SerializeField]
    protected bool bAutoStart = false;
}
