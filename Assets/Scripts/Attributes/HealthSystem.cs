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

    public void ApplyHealthChange(Character damageCauser, float damageAmount)
    {
        if(IsAlive() == true)
        {
            float curHP = hp;
            float newHP = Mathf.Clamp(curHP - damageAmount, 0, maxHP);
            hp = newHP;

            OnHealthChanged?.Invoke(curHP, newHP);

            if (IsAlive() == false)
            {
                OnDeath?.Invoke();
            }
            Debug.LogFormat("Damaged!! Damage Causer : {0} Amount : {1}, Cur HP : {2}", damageCauser.name, damageAmount, hp);
        }        
    }
    public void SetMaxHp(float addAmount)
    {
        float curMaxHP = maxHP;
        float newMaxHP = Mathf.Clamp(curMaxHP + addAmount,0,MAX_HP_LIMIT);
        maxHP = newMaxHP;

        OnMaxHealthChanged?.Invoke(curMaxHP, newMaxHP);
    }
}
