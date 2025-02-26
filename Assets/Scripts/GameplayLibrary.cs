using UnityEngine;

public class GameplayLibrary
{
    public static void ApplyDamage(GameObject damageCauser, GameObject targetActor, float damageAmount)
    {
        HealthSystem targetHealthSystem = targetActor.GetComponent<HealthSystem>();

        if (targetHealthSystem != null)
        {
            targetHealthSystem.ApplyHealthChange(damageCauser, damageAmount);
        }
    }
}
