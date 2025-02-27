using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

public class ActionSystem : MonoBehaviour
{
    public Action<ActionSystem, Action> OnActionStarted;
    public Action<ActionSystem, Action> OnActionStoped;

    protected EGameplayTags activeTags;

    [SerializeField]
    protected List<Action> defaultActions;

    protected List<Action> actions = new List<Action>();

    private Character character = null;

    private void Start()
    {
        character = this.gameObject.GetComponent<Character>();
        foreach (var action in defaultActions)
        {
            AddAction(character, action);
        }
    }
    public Character GetOwnerCharacter()
    {
        return character;
    }
    public void AddAction(Character inInstigator, Action action)
    {
        // 리플렉션 사용하지 않고 type을 이용해 복사 후 각 객체 별 Init처리
        var type = action.GetType();
        Action newAction = (Action)this.gameObject.AddComponent(type);
        newAction.Initialize(this, action);

        actions.Add(newAction);
        
        if(newAction.GetAutoStart() == true && newAction.IsCanStart(inInstigator) == true)
        {
            newAction.StartAction(inInstigator);
        }
    }
    public void RemoveAction(Action action)
    {
        if(action.GetIsRunning() == true)
        {
            return;
        }

        actions.Remove(action);
    }
    public Action GetActionOrNull(EGameplayTags actionTag)
    {
        Assert.IsTrue(isOnlyOneTagSet(actionTag), "Multiple Action Tag in ActivationTag : " + actionTag.ToString());

        foreach (Action action in actions)
        {
            if (action.GetActivationTag() == actionTag)
            {
                return action;
            }
        }
        return null;
    }
    public bool StartActionByTag(Character inInstigator, EGameplayTags actionTag)
    {
        Assert.IsTrue(isOnlyOneTagSet(actionTag), "Multiple Action Tag in ActivationTag");

        Action action = GetActionOrNull(actionTag);
        
        if(action == null)
        {
            return false;
        }
        else
        {
            if(action.IsCanStart(inInstigator) == false)
            {
                Debug.LogWarningFormat("Can not start action {0}. Instigator : {1}, GameObject : {2}", actionTag.ToString(), inInstigator.name, gameObject.name);
                return false;
            }

            EGameplayTags cancelTags = action.GetCanelTags();
            if(cancelTags != EGameplayTags.None_Action)
            {
                foreach (EGameplayTags cancelTag in Enum.GetValues(typeof(EGameplayTags)))
                {
                    if (cancelTag == EGameplayTags.None_Action)
                    {
                        continue;
                    }

                    if (cancelTags.HasFlag(cancelTag) == true)
                    {
                        StopActionByTag(inInstigator, cancelTag);
                    }
                }
            }

            action.StartAction(inInstigator);

            if (OnActionStarted != null)
            {
                OnActionStarted.Invoke(this, action);
            }

            return true;
        }
    }
    public bool StopActionByTag(Character inInstigator, EGameplayTags actionTag)
    {
        Action action = GetActionOrNull(actionTag);
        if(action != null)
        {
            if(action.GetIsRunning() == true)
            {
                action.StopAction(inInstigator);

                if (OnActionStoped != null)
                {
                    OnActionStoped.Invoke(this, action);
                }

                return true;
            }
        }

        return false;
    }
    public bool ActiveTagHasAny(EGameplayTags checkTags)
    {
        return hasAny(activeTags, checkTags);
    }
    protected bool hasAny(EGameplayTags targetTags, EGameplayTags checkTags)
    {
        return (targetTags & checkTags) != EGameplayTags.None_Action;
    }

    public void SetActiveTags(EGameplayTags grantsTags)
    {
        setTags(ref activeTags, grantsTags);
    }
    public void UnSetActiveTags(EGameplayTags grantsTags)
    {
        unsetTags(ref activeTags, grantsTags);
    }
    protected void setTags(ref EGameplayTags targetTags, EGameplayTags setTags)
    {
        targetTags |= setTags;
    }
    protected void unsetTags(ref EGameplayTags targetTags, EGameplayTags unsetTags)
    {
        targetTags &= ~unsetTags;
    }
    protected bool isOnlyOneTagSet(EGameplayTags tag)
    {
        if(tag == EGameplayTags.None_Action)
        {
            return true;
        }

        return (tag != 0) && ((tag & (tag - 1)) == 0);
    }
    public override string ToString()
    {
        StringBuilder sb = new();
        foreach(EGameplayTags tag in Enum.GetValues(typeof(EGameplayTags)))
        {
            if(tag == EGameplayTags.None_Action)
            {
                continue;
            }
            if(activeTags.HasFlag(tag) == true)
            {
                sb.AppendLine(tag.ToString());
            }
        }
            
        return sb.ToString();
    }
}
