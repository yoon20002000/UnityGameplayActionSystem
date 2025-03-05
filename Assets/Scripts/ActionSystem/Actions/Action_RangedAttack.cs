using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class Action_RangedAttack : Action_AttackBase, IApplyActionEffects
{
    [SerializeField]
    protected float maxRange;
    [SerializeField]
    protected LayerMask targetLayerMask;

    public void ApplyActionEffectsToTarget(Character inInstigator)
    {
        Character hitCharacter = getHitGameObjectOrNull(inInstigator, true);
        if (hitCharacter != null)
        {
            applyActionEffactsToTarget(inInstigator, hitCharacter);
        }
    }

    public override void Initialize(ActionSystem InActionSystem, Action other = null)
    {
        base.Initialize(InActionSystem, other);
        Assert.IsTrue(other is Action_RangedAttack);

        Action_RangedAttack action = other as Action_RangedAttack;
        if (action == null)
        {
            return;
        }
        maxRange = action.maxRange;
        targetLayerMask = action.targetLayerMask;
    }
    protected override void attackDelayElapsed(Character inInstigator)
    {
        base.attackDelayElapsed(inInstigator);

        //var col = inInstigator.GetComponent<Collider2D>();
        //Vector2 offset = new Vector2(col.bounds.size.x, 0);
        //getHitGameObjectOrNull(inInstigator, offset);

        Character hitCharacter = getHitGameObjectOrNull(inInstigator, true);

        if (hitCharacter != null)
        {
            GameplayLibrary.ApplyDamage(inInstigator, hitCharacter, damage);
        }

        //applyActionEffactsToTarget(instigator, hitCharacter);
    }
    protected Character getHitGameObjectOrNull(Character inInstigator, bool bIgnoreSelf = true)
    {
        Vector3 start = inInstigator.GetShotTransform().position;
        // 현재는 플레이어 한정
        Ray ray = inInstigator.GetShotRay();

        RaycastHit hit;
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out hit, maxRange))
        {
            targetPoint = hit.point; // 충돌한 지점
            Vector3 shootDirection = (targetPoint - start).normalized;
            Debug.DrawRay(start, shootDirection * maxRange, Color.red, 1f);

            return hit.collider.gameObject.GetComponent<Character>();
        }
        else
        {
            targetPoint = ray.origin + ray.direction * maxRange; // 최대 거리까지 쏘기
            Vector3 shootDirection = (targetPoint - start).normalized;
            Debug.DrawRay(start, shootDirection * maxRange, Color.red, 1f);

            return null;
        }
    }
}
