using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class Action_AttackBase : Action
{
    [SerializeField]
    private float damage;

    [SerializeField]
    private float count;

    [SerializeField]
    private bool bIsDistributionDamage;

    [SerializeField]
    private float attackDelay;
    [SerializeField]
    private float duration;

    private Coroutine attackCo;
    public override void Initialize(ActionSystem InActionSystem, Action other = null)
    {
        base.Initialize(InActionSystem, other);

        Assert.IsTrue(other is Action_AttackBase);
        Action_AttackBase action = other as Action_AttackBase;
        
        if (action == null)
        {
            return;
        }

        damage = action.damage;
        count = action.count;
        bIsDistributionDamage = action.bIsDistributionDamage;
        attackDelay = action.attackDelay;
        duration = action.duration;
    }
    public override void StartAction(GameObject inInstigator)
    {
        // inInstigator.setanim
        base.StartAction(inInstigator);
        attackCo = StartCoroutine(attack(attackDelay, attackDelayElapsed, inInstigator));
    }

    private void OnDestroy()
    {
        if (attackCo != null)
        {
            StopCoroutine(attackCo);
        }
    }

    private IEnumerator attack(float inAttackDelay, System.Action<GameObject> executeFunction, GameObject inInstigator)
    {
        yield return new WaitForSeconds(inAttackDelay);

        executeFunction.Invoke(inInstigator);

        yield return new WaitForSeconds(duration - inAttackDelay);
        StopAction(inInstigator);
    }
    protected virtual void attackDelayElapsed(GameObject inInstigator)
    {

    }

    public override void StopAction(GameObject inInstigator)
    {
        base.StopAction(inInstigator);
    }
}
