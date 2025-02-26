using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ActionSystem : MonoBehaviour
{
    public delegate void OnActionStatedChanged(ActionSystem actionSystem, Action action);

    public OnActionStatedChanged OnActionStated;
    public OnActionStatedChanged OnActionStoped;

    protected GameplayTags activeTags;

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
        if(action.IsRunning() == true)
        {
            return;
        }

        actions.Remove(action);
    }
    [Obsolete] // 얘네는 UObject subclass bp 했을 때 cdo를 각각 생성하는게 아니라 비교 했을때 다 내가 원하는게 아니어도 그 값을 뱉음.
    public Action GetActionOrNull(Action action)
    {
        foreach(Action a in actions)
        {
            if(a.GetType() == action.GetType())
            {
                return a;
            }
        }
        return null;
    }
    public Action GetActionOrNull(GameplayTags actionTag)
    {
        Assert.IsTrue(isOnlyOneTagSet(actionTag), "Multiple Action Tag in ActivationTag");

        foreach (Action action in actions)
        {
            if (action.GetActivationTag() == actionTag)
            {
                return action;
            }
        }
        return null;
    }
    public bool StartActionByTag(Character inInstigator, GameplayTags actionTag)
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

            action.StartAction(inInstigator);
            return true;
        }
    }
    public bool StopActionByTag(Character inInstigator, GameplayTags actionTag)
    {
        Action action = GetActionOrNull(actionTag);
        if(action != null)
        {
            if(action.IsRunning() == true)
            {
                action.StopAction(inInstigator);
                return true;
            }
        }

        return false;
    }
    public bool ActiveTagHasAny(GameplayTags checkTags)
    {
        return hasAny(activeTags, checkTags);
    }
    protected bool hasAny(GameplayTags targetTags, GameplayTags checkTags)
    {
        return (targetTags & checkTags) != GameplayTags.None_Action;
    }

    public void SetActiveTags(GameplayTags grantsTags)
    {
        setTags(activeTags, grantsTags);
    }
    public void UnSetActiveTags(GameplayTags grantsTags)
    {
        unsetTags(activeTags, grantsTags);
    }
    protected void setTags(GameplayTags targetTags, GameplayTags setTags)
    {
        targetTags |= setTags;
    }
    protected void unsetTags(GameplayTags targetTags, GameplayTags unsetTags)
    {
        targetTags &= ~unsetTags;
    }

    protected bool isOnlyOneTagSet(GameplayTags tag)
    {
        return (tag != 0) && ((tag & (tag - 1)) == 0);
    }
}
