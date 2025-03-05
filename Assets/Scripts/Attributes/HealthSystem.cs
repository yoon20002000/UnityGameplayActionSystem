using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public static readonly float MAX_HP_LIMIT = 999;

    public System.Action<float, float> OnHealthChanged;
    public System.Action<float, float> OnMaxHealthChanged;
    public System.Action OnDeath;

    public bool IsAlive() => hp > 0;
    public float GetHPPercentage() => hp / maxHP;
    public HealthSystem()
    {
        maxHP = 100;
        hp = maxHP;
    }
    public HealthSystem(float hp, float maxHP)
    {
        this.hp = hp;
        this.maxHP = maxHP;
    }

    public void ApplyHealthChange(Character damageCauser, float damageAmount)
    {
        if (bIsInvincibility == true && damageAmount > 0)
        {
            return;
        }

        if (IsAlive() == true)
        {
            float curHP = hp;
            float newHP = Mathf.Clamp(curHP - damageAmount, 0, maxHP);
            hp = newHP;

            if(OnHealthChanged != null)
            {
                OnHealthChanged.Invoke(curHP, newHP);
            }

            if (OnDeath != null && IsAlive() == false)
            {
                OnDeath.Invoke();
            }
            Debug.LogFormat("Damaged!! Damage Causer : {0} Amount : {1}, Cur HP : {2}", damageCauser.name, damageAmount, hp);
        }
    }
    public void AddMaxHp(float addAmount)
    {
        float curMaxHP = maxHP;
        float newMaxHP = Mathf.Clamp(curMaxHP + addAmount, 0, MAX_HP_LIMIT);
        maxHP = newMaxHP;

        if(OnMaxHealthChanged != null)
        {
            OnMaxHealthChanged.Invoke(curMaxHP, newMaxHP);
        }
    }

    private float hp;
    public float GetHP()
    {
        return hp;
    }
    [SerializeField]
    private float maxHP;
    public float GetMaxHP()
    {
        return maxHP;
    }
    private bool bIsInvincibility = false;
    public void SetInvincibility(bool bInvincibility)
    {
        bIsInvincibility = bInvincibility;
    }
    public bool GetInvincibility()
    {
        return bIsInvincibility;
    }
}
