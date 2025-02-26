using System;
using UnityEngine;

public class ActionSystem : MonoBehaviour
{
    public delegate void OnActionStatedChanged(ActionSystem actionSystem, Action action);

    public OnActionStatedChanged OnActionStated;
    public OnActionStatedChanged OnActionStoped;

    protected GameplayTags activeTags;

    
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
}
