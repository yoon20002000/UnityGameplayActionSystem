using UnityEngine.Assertions;
using UnityEngine;
using System.Collections.Generic;

public class Action : MonoBehaviour
{
    public Action()
    {
        activationTag = EGameplayTags.None_Action;
        grantsTags = EGameplayTags.None_Action;
        cancelTags = EGameplayTags.None_Action;
        blockedTags = EGameplayTags.None_Action;
    }
    public virtual void DeepCopy(Action other)
    {
        Assert.IsNotNull(other, "You DeepCopy using original action resources");

        this.actionSystem = other.actionSystem;
        this.activationTag = other.activationTag;
        this.grantsTags = other.grantsTags;
        this.cancelTags = other.cancelTags;
        this.blockedTags = other.blockedTags;
        this.bIsRunning = other.bIsRunning;
        this.bAutoStart = other.bAutoStart;
        this.timeStarted = other.timeStarted;
        this.instigator = other.instigator;
        applyActionEffects.Clear();
        applyActionEffects.AddRange(other.applyActionEffects);
    }
    public virtual void Initialize(ActionSystem InActionSystem, Action other = null)
    {
        actionSystem = InActionSystem;
        if(other != null)
        {
            this.activationTag = other.activationTag;
            this.grantsTags = other.grantsTags;
            this.cancelTags = other.cancelTags;
            this.blockedTags = other.blockedTags;
            this.bIsRunning = other.bIsRunning;
            this.bAutoStart = other.bAutoStart;
            this.timeStarted = other.timeStarted;
            this.instigator = other.instigator;
            applyActionEffects.Clear();
            applyActionEffects.AddRange(other.applyActionEffects);
        }
    }

    public ActionSystem GetActionSystem()
    {
        return actionSystem;
    }

    public EGameplayTags GetActivationTag()
    {
        return activationTag;
    }
    public EGameplayTags GetGrantsTags()
    {
        return grantsTags;
    }
    public EGameplayTags GetCanelTags()
    {
        return cancelTags;
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
        Debug.Log("Stoped action : " + this.activationTag.ToString());
        actionSystem.UnSetActiveTags(grantsTags);

        bIsRunning = false;
        instigator = inInstigator;

        actionSystem.OnActionStoped?.Invoke(actionSystem, this);
    }

    protected ActionSystem actionSystem = null;

    // editor에서 하나만 선택하게 해야 됨.
    [SerializeField]
    protected EGameplayTags activationTag;

    [SerializeField]
    protected EGameplayTags grantsTags;
    [SerializeField]
    protected EGameplayTags cancelTags;
    [SerializeField]
    protected EGameplayTags blockedTags;

    [SerializeField]
    protected bool bIsRunning = false;
    protected Character instigator = null;

    protected float timeStarted;

    [SerializeField]
    protected bool bAutoStart = false;

    [SerializeField]
    protected List<ActionEffect> applyActionEffects = new();
}
