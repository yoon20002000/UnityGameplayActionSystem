using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackActionData", menuName = "Game Action System/RangedAttackActionData")]
public class RangedAttackActionData : AttackActionData
{
    public float maxRange;
    public LayerMask targetLayerMask;
    public override ActionInstance CreateInstacne(ActionSystem system)
    {
        return new RangedAttackActionInstance(this, system);
    }
}
