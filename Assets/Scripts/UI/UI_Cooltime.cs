using UnityEngine;
using UnityEngine.UI;

public class UI_Cooltime : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private Image cooldownIconImage;

    [SerializeField]
    private EGameplayTags targetActionTag;

    [Space(10)]
    [SerializeField]
    private ActionSystem targetActionSystem;

    private void Start()
    {
        
    }
    private void Update()
    {
        if(targetActionSystem != null && targetActionSystem.GetActionOrNull(targetActionTag) != null)
        {
            cooldownIconImage.fillAmount = 1 - targetActionSystem.GetActionOrNull(targetActionTag).GetRemainPercentage();
        }
    }
}
