using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

public class ActionSystem : MonoBehaviour
{
    public Action<ActionSystem, ActionInstance> OnActionStarted;
    public Action<ActionSystem, ActionInstance> OnActionStoped;

    [SerializeField]
    protected List<ActionData> defaultActions;

    private Dictionary<EGameplayTags, ActionInstance> actions = new();
    private List<ActionInstance> activeGameEffects = new();
    private EGameplayTags activeTags;
    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
        activeGameEffects.Clear();
        foreach (var data in defaultActions)
        {
            AddAction(character, data);
        }
    }
    public void AddAction(Character instigator, ActionData data)
    {
        if(data == null || actions.ContainsKey(data.ActivationTag))
        {
            return;
        }

        var instance = data.CreateInstacne(this);
        actions[data.ActivationTag] = instance;

        if(data.bAutoStart && instance.CanStart())
        {
            StartActionByTag(instigator, data.ActivationTag);
        }
    }
    public bool StartActionByTag(Character instigator, EGameplayTags tag)
    {
        if(!actions.TryGetValue(tag, out var instance) || !instance.CanStart())
        {
            return false;
        }

        foreach(EGameplayTags cancelTag in Enum.GetValues(typeof(EGameplayTags)))
        {
            if(cancelTag != EGameplayTags.None_Action && instance.Data.CancelTags.HasFlag(cancelTag))
            {
                StopActionByTag(instigator, cancelTag);
            }   
        }
        instance.Start(instigator);
        if (OnActionStarted != null)
        {
            OnActionStarted.Invoke(this, instance);
        }
        return true;
    }
    public bool StopActionByTag(Character instigator, EGameplayTags tag)
    {
        Assert.IsTrue(tag != EGameplayTags.None_Action);
        if(actions.TryGetValue(tag, out var instance) && instance.IsRunning)
        {
            instance.Stop(instigator);
            if(OnActionStoped != null)
            {
                OnActionStoped.Invoke(this, instance);
            }
            return true;
        }
        return false;
    }
    public void ApplyGameEffect(Character instigator, ActionData data)
    {
        ActionInstance instance = data.CreateInstacne(this);
        activeGameEffects.Add(instance);

        instance.Start(instigator);
        if (OnActionStarted != null)
        {
            OnActionStarted.Invoke(this, instance);
        }
    }
    public void RemoveGameEffect(Character instigator, ActionInstance effectInstance)
    {
        if(activeGameEffects.Remove(effectInstance))
        {
            effectInstance.Stop(instigator);
            if (OnActionStoped != null)
            {
                OnActionStoped.Invoke(this, effectInstance);
            }
        }
    }
    public Character GetOwnerCharacter()
    {
        return character;
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
