using UnityEngine;

public class GameplayLibrary
{
    public static void ApplyDamage(Character damageCauser, Character targetActor, float damageAmount)
    {
        if(targetActor == null)
        {
            return;
        }

        HealthSystem targetHealthSystem = targetActor.GetHealthSystem();

        targetHealthSystem.ApplyHealthChange(damageCauser, damageAmount);
    }
}
