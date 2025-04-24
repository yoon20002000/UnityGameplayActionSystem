using System.Collections;
using UnityEngine;

public class AttackActionInstance : ActionInstance
{
    private Coroutine attackCo;
    public AttackActionInstance(ActionData data, ActionSystem system) : base(data, system)
    {
        
    }
    public override void Start(Character instigator)
    {
        base.Start(instigator);
        if( Data is AttackActionData attackData)
        {
            attackCo = actionSystem.StartCoroutine(AttackCoroutine(attackData.attackDelay, ExecuteAttack, instigator));
        }
        else
        {
            Debug.LogError("Invalid action data");
        }
    }
    private IEnumerator AttackCoroutine(float attackDelay, System.Action<Character> exeCuteFunction, Character instigator)
    {
        yield return Awaitable.WaitForSecondsAsync(attackDelay);
        exeCuteFunction.Invoke(instigator);

        if(Data is AttackActionData attackData)
        {
            yield return Awaitable.WaitForSecondsAsync(attackData.duration - attackDelay);
            Stop(instigator);
        }
    }

    protected virtual void ExecuteAttack(Character instigator)
    {

    }
}
