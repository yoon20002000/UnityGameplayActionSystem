using UnityEngine;

public class ActionInstance
{
    public ActionData Data { get; private set; }
    public Character Instigator { get; private set; }

    public float TimeStarted { get; private set; }
    public float EndCooldown { get; private set; }
    public bool IsRunning { get; private set; }
    public float Value { get; private set; }

    protected ActionSystem actionSystem;

    public ActionInstance(ActionData data, ActionSystem system)
    {
        Data = data;
        InitData(Data);
        actionSystem = system;
    }

    public bool CanStart()
    {
        return !IsRunning && !IsCoolingdown() && !actionSystem.ActiveTagHasAny(Data.BlockedTags);
    }

    public virtual void Start(Character instigator)
    {
        if (!CanStart())
        {
            return;
        }

        Instigator = instigator;

        IsRunning = true;
        TimeStarted = Time.time;
        EndCooldown = TimeStarted + Data.CoolTime;

        actionSystem.SetActiveTags(Data.GrantsTags);
        ApplyStartAnimation(instigator);
        
        IApplyActionEffectsSomeTarget applyActionEffectsInterface = this as IApplyActionEffectsSomeTarget;

        if (applyActionEffectsInterface != null)
        {
            applyActionEffectsInterface.ApplyActionEffectsToTarget(instigator);
        }
        else
        {
            ApplyEffects();
        }

        if (Data.bAutoStop)
        {
            actionSystem.StopActionByTag(instigator, Data.ActivationTag);
        }
    }
    public virtual void Stop(Character instigator)
    {
        DefaultStop(instigator);

        actionSystem.StopActionByTag(instigator, Data.ActivationTag);
    }
    public bool IsCoolingdown() => Time.time < EndCooldown;

    private void ApplyEffects()
    {
        foreach (var effect in Data.ApplyActionEffects)
        {
            actionSystem.ApplyGameEffect(Instigator, effect);
        }
    }
    private void ApplyStartAnimation(Character character) => ApplyAnimation(character.GetAnimatorOrNull(), Data.StartAnimations);
    private void ApplyStopAnimation(Character character) => ApplyAnimation(character.GetAnimatorOrNull(), Data.StopAnimations);

    private void ApplyAnimation(Animator animator, AnimationDatas data)
    {
        if (animator == null)
        {
            return;
        }

        foreach (var kv in data.boolData.AnimationDatas)
        {
            animator.SetBool(kv.Key, kv.Value); 
        }
        foreach (var kv in data.intData.AnimationDatas)
        {
            animator.SetInteger(kv.Key, kv.Value); 
        }
        foreach (var kv in data.floatData.AnimationDatas)
        {
            animator.SetFloat(kv.Key, kv.Value); 
        }
        foreach (var kv in data.stringData.AnimationDatas)
        {
            animator.SetTrigger(kv.Key); 
        }
    }
    private void InitData(ActionData data)
    {
        Value = data.value;
    }
    protected void DefaultStop(Character instigator)
    {
        if (!IsRunning)
        {
            return;
        }

        IsRunning = false;
        actionSystem.UnSetActiveTags(Data.GrantsTags);
        ApplyStopAnimation(Instigator);
        Instigator = instigator;
    }

    protected virtual void ApplyActionEffectsToTarget(Character instigator, Character targetCharacter)
    {

        if (targetCharacter == null)
        {
            Debug.LogError("Target Character is null!! " + Data.ActivationTag);
            return;
        }

        ActionSystem targetActionSystem = targetCharacter.GetActionSystem();

        if (targetActionSystem == null)
        {
            return;
        }

        foreach (var actionEffect in Data.ApplyActionEffects)
        {
            targetActionSystem.ApplyGameEffect(instigator, actionEffect);
        }
    }
}
