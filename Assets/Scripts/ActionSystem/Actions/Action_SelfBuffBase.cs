using UnityEngine;

public class Action_SelfBuffBase : Action, IApplyActionEffects
{
    public void ApplyActionEffectsToTarget(Character inInstigator)
    {
        applyActionEffactsToTarget(inInstigator, actionSystem.GetOwnerCharacter());
    }
}
