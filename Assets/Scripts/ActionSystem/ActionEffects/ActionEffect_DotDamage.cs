using UnityEngine;
using UnityEngine.Assertions;

public class ActionEffect_DotDamage : ActionEffect
{
    [SerializeField]
    private float damage;
    public override void DeepCopy(Action other)
    {
        base.DeepCopy(other);
        Assert.IsTrue(other is ActionEffect_DotDamage);

        ActionEffect_DotDamage actionEffect = other as ActionEffect_DotDamage;
        if (actionEffect == null)
        {
            return;
        }
        damage = actionEffect.damage;
    }
    public override bool IsCanStart(Character inInstigator)
    {
        return base.IsCanStart(inInstigator) == true && GetActionSystem().GetOwnerCharacter().GetHealthSystem().IsAlive() == true;
    }
    public override void Initialize(ActionSystem InActionSystem, Action other = null)
    {
        base.Initialize(InActionSystem, other);

        ActionEffect_DotDamage actionEffect = other as ActionEffect_DotDamage;
        if (actionEffect != null)
        {
            damage = actionEffect.damage;
        }
    }
    protected override void ExecutePeriodEffect(Character inInstigator)
    {
        base.ExecutePeriodEffect(inInstigator);

        GameplayLibrary.ApplyDamage(inInstigator, actionSystem.GetOwnerCharacter(), damage);
    }
}
