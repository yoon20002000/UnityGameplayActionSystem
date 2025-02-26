using NUnit.Framework;
using UnityEngine;

public class Action_RangedAttack : Action_AttackBase
{
    [SerializeField]
    protected float distance;
    [SerializeField]
    protected LayerMask targetLayerMask;
    public override void Initialize(ActionSystem InActionSystem, Action other = null)
    {
        base.Initialize(InActionSystem, other);
        Assert.IsTrue(other is Action_RangedAttack);

        Action_RangedAttack action = other as Action_RangedAttack;
        if (action == null)
        {
            return;
        }
        distance = action.distance;
        targetLayerMask = action.targetLayerMask;
    }
    protected override void attackDelayElapsed(GameObject inInstigator)
    {
        base.attackDelayElapsed(inInstigator);

        //var col = inInstigator.GetComponent<Collider2D>();
        //Vector2 offset = new Vector2(col.bounds.size.x, 0);
        //getHitGameObjectOrNull(inInstigator, offset);

        GameObject hitGameObject = getHitGameObjectOrNull(inInstigator, true);
        
        if (hitGameObject != null)
        {
            GameplayLibrary.ApplyDamage(inInstigator, hitGameObject, damage);
        }
    }
    protected GameObject getHitGameObjectOrNull(GameObject inInstigator, Vector2 offset = default)
    {
        Vector2 start = inInstigator.transform.position;
        start += offset;
        Vector2 dir = instigator.transform.right;
        Vector2 end = start + dir * distance;

        RaycastHit2D hit = Physics2D.Raycast(start, dir, distance, targetLayerMask);
        Debug.DrawRay(start, dir * distance, Color.red, 5);

        if(hit)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
    protected GameObject getHitGameObjectOrNull(GameObject inInstigator, bool bIgnoreSelf = true)
    {
        Vector2 start = inInstigator.transform.position;
        Vector2 dir = instigator.transform.right;
        RaycastHit2D[] hits = Physics2D.RaycastAll(start, dir, distance, targetLayerMask);
        Debug.DrawRay(start, dir * distance, Color.red, 3);
        foreach (var hit in hits)
        {
            if (hit.collider.gameObject != inInstigator)
            {
                Debug.LogFormat("Hit Target : {0}", hit.collider.gameObject.name);
                return hit.collider.gameObject;
            }
        }
        return null;
    }
}
