using UnityEngine;

public class Action_SelfBuffBase : Action, IApplyActionEffectsSomeTarget
{
    public void ApplyActionEffectsToTarget(Character inInstigator)
    {
        applyActionEffactsToTarget(inInstigator, actionSystem.GetOwnerCharacter());
    }
}
