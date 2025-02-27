using UnityEngine;

public class Action_SelfBuffBase : Action, IApplyActionEffects
{
    public void ApplyActionEffactsToTarget(Character inInstigator)
    {
        applyActionEffactsToTarget(inInstigator, actionSystem.GetOwnerCharacter());
    }
}
