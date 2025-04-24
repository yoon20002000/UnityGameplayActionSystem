using UnityEngine.Assertions;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[Obsolete("ActionInstance로 대체 작업 중")]
public class Action : MonoBehaviour
{
    public Action()
    {
        activationTag = EGameplayTags.None_Action;
        grantsTags = EGameplayTags.None_Action;
        cancelTags = EGameplayTags.None_Action;
        blockedTags = EGameplayTags.None_Action;
    }
    
    
    private void Start()
    {
        if(applyActionEffects.Count > 0 )
        {
            Assert.IsTrue(this is IApplyActionEffects, "If you want to apply effect than implement IApplyActionEffects interface");
        }
    }
    public virtual void DeepCopy(Action other)
    {
        Assert.IsNotNull(other, "In action is null");

        this.actionSystem = other.actionSystem;
        this.activationTag = other.activationTag;
        this.grantsTags = other.grantsTags;
        this.cancelTags = other.cancelTags;
        this.blockedTags = other.blockedTags;
        this.bIsRunning = other.bIsRunning;
        this.bAutoStart = other.bAutoStart;
        this.timeStarted = other.timeStarted;
        this.instigator = other.instigator;
        this.bAutoStopAfterOnce = other.bAutoStopAfterOnce;
        this.coolTime = other.coolTime;
        applyActionEffects.Clear();
        applyActionEffects.AddRange(other.applyActionEffects);
        startAnimationDatas = other.startAnimationDatas;
        stopAnimationDatas = other.stopAnimationDatas;
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
            this.bAutoStopAfterOnce = other.bAutoStopAfterOnce;
            this.coolTime = other.coolTime;
            applyActionEffects.Clear();
            applyActionEffects.AddRange(other.applyActionEffects);
            startAnimationDatas = other.startAnimationDatas;
            stopAnimationDatas = other.stopAnimationDatas;
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
    public bool GetIsRunning()
    {
        return bIsRunning;
    }

    public virtual bool IsCanStart(Character inInstigator)
    {
        if (GetIsRunning() == true || GetIsCooltime() == true)
        {
            return false;
        }

        if (actionSystem.ActiveTagHasAny(blockedTags))
        {
            return false;
        }

        return true;
    }
    
    public bool GetIsCooltime() => endCooltime > Time.time;
    public void ResetCooltime()
    {
        endCooltime = Time.time;
    }
    public float GetRemainCooltime() => endCooltime - Time.time;
    public float GetRemainPercentage()
    {
        return GetRemainCooltime() / coolTime;
    }
    public virtual void StartAction(Character inInstigator)
    {
        Debug.LogFormat("Start Action : {0}, Instigator : {1}", activationTag.ToString(), inInstigator.name );
        actionSystem.SetActiveTags(grantsTags);

        bIsRunning = true;
        instigator = inInstigator;

        timeStarted = Time.time;
        endCooltime = timeStarted + coolTime;
        applyStartAnimation(inInstigator.GetAnimatorOrNull());

        IApplyActionEffects applyActionEffectsInterface = this as IApplyActionEffects;
        
        if(applyActionEffectsInterface != null)
        {
            applyActionEffectsInterface.ApplyActionEffectsToTarget(inInstigator);
        }

        if (bAutoStopAfterOnce == true)
        {
            StopAction(inInstigator);
        }
    }

    public virtual void StopAction(Character inInstigator)
    {
        Debug.Log("Stoped action : " + this.activationTag.ToString());
        actionSystem.UnSetActiveTags(grantsTags);
        
        bIsRunning = false;
        
        applyStopAnimationData(inInstigator.GetAnimatorOrNull());

        instigator = inInstigator;
    }
    protected virtual void applyActionEffactsToTarget(Character inInstigator, Character targetCharacter)
    {
        if(targetCharacter == null)
        {
            Debug.LogError("Target Character is null!! " + activationTag.ToString());
            return;
        }

        ActionSystem targetActionSystem = targetCharacter.GetActionSystem();
        
        if(targetActionSystem == null)
        {
            return;
        }

        foreach (var actionEffect in applyActionEffects)
        {
            //targetActionSystem.AddAction(inInstigator, actionEffect);
        }
    }

    protected void applyStartAnimation(Animator targetAnimator)
    {
        applyAnimationData(targetAnimator, ref startAnimationDatas);
    }
    protected void applyStopAnimationData(Animator targetAnimator)
    {
        applyAnimationData(targetAnimator, ref stopAnimationDatas);
    }

    protected void applyAnimationData(Animator targetAnimator, ref AnimationDatas animationDatas)
    {
        if (targetAnimator == null)
        {
            return;
        }

        foreach (var animData in animationDatas.boolData.AnimationDatas)
        {
            targetAnimator.SetBool(animData.Key, animData.Value);
        }

        foreach (var animData in animationDatas.intData.AnimationDatas)
        {
            targetAnimator.SetInteger(animData.Key, animData.Value);
        }

        foreach (var animData in animationDatas.floatData.AnimationDatas)
        {
            targetAnimator.SetFloat(animData.Key, animData.Value);
        }

        foreach (var animData in animationDatas.stringData.AnimationDatas)
        {
            targetAnimator.SetTrigger(animData.Key);
        }
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

    protected bool bIsRunning = false;
    protected Character instigator = null;

    protected float timeStarted;

    [SerializeField]
    protected bool bAutoStart = false;
    [SerializeField]
    protected bool bAutoStopAfterOnce = false;
    [SerializeField]
    protected List<ActionEffect> applyActionEffects = new();

    [SerializeField]
    protected float coolTime;
    protected float endCooltime;
    protected bool bIsCoolingdown;

    [SerializeField]
    protected AnimationDatas startAnimationDatas;
    [SerializeField]
    protected AnimationDatas stopAnimationDatas;
}
