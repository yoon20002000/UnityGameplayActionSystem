using UnityEngine;

public class RangedAttackActionInstance : AttackActionInstance, IApplyActionEffects
{
    public RangedAttackActionInstance(ActionData data, ActionSystem system) : base(data, system)
    {
    }

    public void ApplyActionEffectsToTarget(Character inInstigator)
    {
        if (Data is RangedAttackActionData rangedData)
        {
            Character hitCharacter = GetHitCharacter(inInstigator, rangedData.maxRange, rangedData.targetLayerMask);
            if (hitCharacter != null)
            {
                ApplyActionEffectsToTarget(inInstigator, hitCharacter);
            }
        }
    }
    protected override void ExecuteAttack(Character inInstigator)
    {
        base.ExecuteAttack(inInstigator);

        if (Data is RangedAttackActionData rangedData)
        {
            Character hitCharacter = GetHitCharacter(inInstigator, rangedData.maxRange, rangedData.targetLayerMask);
            if (hitCharacter != null)
            {
                GameplayLibrary.ApplyDamage(inInstigator, hitCharacter, rangedData.damage);
            }
        }
    }
    protected Character GetHitCharacter(Character inInstigator, float maxRange, LayerMask targetLayerMask)
    {
        Vector3 start = inInstigator.GetShotTransform().position;
        Ray ray = inInstigator.GetShotRay();

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxRange, targetLayerMask))
        {
            Debug.DrawRay(start, (hit.point - start).normalized * maxRange, Color.red, 1f);
            return hit.collider.gameObject.GetComponent<Character>();
        }

        return null;
    }
}
