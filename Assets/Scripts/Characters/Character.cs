using UnityEngine;
[RequireComponent(typeof(ActionSystem))]
[RequireComponent(typeof(HealthSystem))]
public class Character : MonoBehaviour
{
    [SerializeField]
    protected ActionSystem actionSystem;
    public ActionSystem GetActionSystem()
    {
        return actionSystem;
    }
    [SerializeField]
    protected HealthSystem healthSystem;
    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}
