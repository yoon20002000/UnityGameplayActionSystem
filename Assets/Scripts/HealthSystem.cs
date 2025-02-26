using UnityEngine;

public class HealthSystem : MonoBehaviour
{
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
    private float maxHP;
    public float GetMaxHP()
    {
        return maxHP;
    }
}
