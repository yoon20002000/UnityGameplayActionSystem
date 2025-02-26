using System.Collections;
using UnityEngine;

public class ActionEffect : Action
{
    ActionEffect():base()
    {
        bAutoStart = true;
    }
    
    public override void StartAction(GameObject inInstigator)
    {
        base.StartAction(inInstigator);

        if(duration > 0.0f)
        {
            durationCo = StartCoroutine(executeAction(duration, inInstigator, StopAction));
        }
        
        ExecutePeriodEffect(inInstigator);

        if (period > 0.0f)
        {
            periodCo = StartCoroutine(executeAction(period, inInstigator, ExecutePeriodEffect));
        }
    }
    public override void StopAction(GameObject inInstigator)
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

    protected virtual void ExecutePeriodEffect(GameObject inInstigator)
    {

    }

    protected IEnumerator executeAction(float waitSec, GameObject inInstigator, System.Action<GameObject> action)
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
