using GameplayActionSystem;
using System.Collections;
using UnityEngine;

public class ActionEffectInstance : ActionInstance
{
    public float Duration { get; private set; }
    public float Period { get; private set; }

    private Coroutine durationCo;
    private Coroutine periodCo;

    public ActionEffectInstance(ActionEffectData data, ActionSystem system) : base(data, system)
    {
        Duration = data.duration;
        Period = data.period;
    }

    public override void Start(Character instigator)
    {
        base.Start(instigator);

        ExecuteEffect(instigator);

        if(Period > 0)
        {
            periodCo = actionSystem.StartCoroutine(PeriodicEffect(instigator));
        }
        if(Duration > 0)
        {
            durationCo = actionSystem.StartCoroutine(EndAfterDuration(instigator));
        }
    }
    public override void Stop(Character instigator)
    {
        DefaultStop(instigator);

        if (periodCo != null)
        {
            actionSystem.StopCoroutine(periodCo);
        }

        if(durationCo != null)
        {
            actionSystem.StopCoroutine(durationCo);
        }

        durationCo = null;
        periodCo = null;
    }

    private IEnumerator PeriodicEffect(Character instigator)
    {
        while(true)
        {
            yield return Awaitable.WaitForSecondsAsync(Period);
            ExecuteEffect(instigator);
        }
    }

    private IEnumerator EndAfterDuration(Character instigator)
    {
        yield return Awaitable.WaitForSecondsAsync(Duration);
        actionSystem.RemoveGameEffect(instigator, this);
    }
    protected virtual void ExecuteEffect(Character instigator)
    {
        if (Data is ActionEffectData effectData)
        {
            switch (effectData.effectType)
            {
                case EffectType.Heal:
                    ApplyHeal(instigator, effectData);
                    break;
                case EffectType.Damage:
                    ApplyDamage(instigator, effectData);
                    break;
                case EffectType.Buff:
                    ApplyBuff(instigator, effectData);
                    break;
                default:
                    Debug.LogWarning("Unhandled EffectType: " + effectData.effectType);
                    break;
            }
        }
        else
        {
            Debug.LogError("Data is not of type ActionEffectData!");
        }
    }
    private void ApplyHeal(Character instigator, ActionEffectData effectData)
    {
        GameplayLibrary.ApplyDamage(instigator, actionSystem.GetOwnerCharacter(), -effectData.value);
    }

    private void ApplyDamage(Character instigator, ActionEffectData effectData)
    {
        GameplayLibrary.ApplyDamage(instigator, actionSystem.GetOwnerCharacter(), effectData.value);
    }

    private void ApplyBuff(Character instigator, ActionEffectData effectData)
    {
        
    }
}
