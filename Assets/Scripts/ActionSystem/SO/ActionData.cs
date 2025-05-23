using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionData", menuName = "Game Action System/ActionData")]
public class ActionData : ScriptableObject
{
    public EGameplayTags ActivationTag;
    public EGameplayTags GrantsTags;
    public EGameplayTags CancelTags;
    public EGameplayTags BlockedTags;
    public bool bAutoStart = false;
    public bool bAutoStop = false;
    public float value;
    public float CoolTime;

    public List<ActionEffectData> ApplyActionEffects;

    public AnimationDatas StartAnimations;
    public AnimationDatas StopAnimations;

    // ActionData 단독으로 안쓰기로 확정시
    // public abstract ActionInstance CreateInstance(ActionSystem system);

    public virtual ActionInstance CreateInstance(ActionSystem system)
    {
        return new ActionInstance(this, system);
    }
}
