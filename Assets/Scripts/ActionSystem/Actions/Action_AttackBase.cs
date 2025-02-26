using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Action_AttackBase : Action
{
    [SerializeField]
    protected float damage;

    [SerializeField]
    protected float count;

    [SerializeField]
    protected bool bIsDistributionDamage;

    [SerializeField]
    protected float attackDelay;
    [SerializeField]
    protected float duration;

    [SerializeField]
    protected List<ActionEffect> applyActionEffects = new ();

    protected Coroutine attackCo;
    public override void DeepCopy(Action other)
    {
        base.DeepCopy(other);

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
        applyActionEffects.Clear();
        applyActionEffects.AddRange(action.applyActionEffects);
    }
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
        applyActionEffects.Clear();
        applyActionEffects.AddRange(action.applyActionEffects);
    }
    public override void StartAction(Character inInstigator)
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

    private IEnumerator attack(float inAttackDelay, System.Action<Character> executeFunction, Character inInstigator)
    {
        yield return new WaitForSeconds(inAttackDelay);

        executeFunction.Invoke(inInstigator);

        yield return new WaitForSeconds(duration - inAttackDelay);
        StopAction(inInstigator);
    }
    protected virtual void attackDelayElapsed(Character inInstigator)
    {
        
    }
    protected virtual void applyActionEffactsToTarget(Character inInstigator, Character targetCharacter)
    {
        ActionSystem targetActionSystem = targetCharacter.GetActionSystem();
        foreach (var actionEffect in applyActionEffects)
        {
            targetActionSystem.AddAction(inInstigator, actionEffect);
        }
    }
    public override void StopAction(Character inInstigator)
    {
        base.StopAction(inInstigator);
    }
}
