using UnityEngine;

[CreateAssetMenu(fileName = "AttackActionData", menuName = "Game Action System/AttackActionData")]
public class AttackActionData : ActionData
{
    public float damage;
    public float count;
    public bool bIsDistributionDamage;
    public float attackDelay;
    public float duration;
    public override ActionInstance CreateInstance(ActionSystem system)
    {
        return new AttackActionInstance(this, system);
    }
}
