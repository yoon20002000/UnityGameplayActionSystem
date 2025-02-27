using TMPro;
using UnityEngine;

public class UI_Main : MonoBehaviour
{
    [SerializeField]
    Player mainPlayer;
    [SerializeField]
    TextMeshProUGUI gameplayTagText;
    private void Update()
    {
        if(gameplayTagText != null && mainPlayer != null)
        {
            gameplayTagText.text = mainPlayer.GetActionSystem().ToString();
        }
    }
}
