using UnityEngine;

public class GameplayLibrary
{
    public static void ApplyDamage(Character damageCauser, Character targetActor, float damageAmount)
    {
        HealthSystem targetHealthSystem = targetActor.GetComponent<HealthSystem>();

        if (targetHealthSystem != null)
        {
            targetHealthSystem.ApplyHealthChange(damageCauser, damageAmount);
        }
    }
}
