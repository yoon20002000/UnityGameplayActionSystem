using System.Collections;
using UnityEngine;

public class ActionEffect : Action
{
    public ActionEffect() : base()
    {
        bAutoStart = true;
    }
    public override void DeepCopy(Action other)
    {
        base.DeepCopy(other);
    }

    public override void Initialize(ActionSystem InActionSystem, Action other = null)
    {
        base.Initialize(InActionSystem, other);
    }
    public override void StartAction(Character inInstigator)
    {
        base.StartAction(inInstigator);

        if (duration > 0.0f)
        {
            durationCo = StartCoroutine(executeAction(duration, inInstigator, StopAction));
        }

        ExecutePeriodEffect(inInstigator);

        if (period > 0.0f)
        {
            periodCo = StartCoroutine(executeAction(period, inInstigator, ExecutePeriodEffect));
        }
    }
    public override void StopAction(Character inInstigator)
    {
        base.StopAction(inInstigator);

        StopCoroutine(durationCo);
        StopCoroutine(periodCo);

        actionSystem.RemoveAction(this);
    }

    public float GetRemainingTime()
    {
        float endTime = timeStarted + duration;
        return endTime - Time.time;
    }

    protected virtual void ExecutePeriodEffect(Character inInstigator)
    {

    }

    protected IEnumerator executeAction(float waitSec, Character inInstigator, System.Action<Character> action)
    {
        yield return new WaitForSeconds(waitSec);
        action.Invoke(inInstigator);
    }

    [SerializeField]
    private float duration;
    [SerializeField]
    private float period;

    Coroutine durationCo;
    Coroutine periodCo;
}
