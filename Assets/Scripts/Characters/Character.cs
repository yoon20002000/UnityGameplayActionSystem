using UnityEngine;
[RequireComponent(typeof(HealthSystem))]
public class Character : MonoBehaviour
{
    [SerializeField]
    protected HealthSystem healthSystem;
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}
