using UnityEngine;

public class Action_RangedAttack : Action_AttackBase
{
    [SerializeField]
    private float distance;
    [SerializeField]
    private LayerMask targetLayerMask;
    protected override void attackDelayElapsed(GameObject inInstigator)
    {
        base.attackDelayElapsed(inInstigator);

        Vector2 start = inInstigator.transform.position;
        Vector2 dir = instigator.transform.right;
        Vector2 end = start + dir * distance;
        RaycastHit2D hit = Physics2D.Raycast(start, dir, distance, targetLayerMask);
        Debug.DrawRay(start, dir * distance, Color.red, 1);
        if (hit)
        {
            Debug.Log("Hit Object : " + hit.collider.gameObject.name);
        }
    }
}
