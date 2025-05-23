using GameplayActionSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionEffectData", menuName = "Game Action System/ActionEffectData")]
public class ActionEffectData : ActionData
{
    public float duration;
    public float period;

    public EffectType effectType;
    public override ActionInstance CreateInstance(ActionSystem system)
    {
        return new ActionEffectInstance(this,system);
    }
}
