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
    public override void StartAction(GameObject inInstigator)
    {
        // inInstigator.setanim
        attackCo = StartCoroutine(attack(attackDelay, attackDelayElapsed, inInstigator));

        base.StartAction(inInstigator);
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
