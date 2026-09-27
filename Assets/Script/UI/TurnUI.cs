using UnityEngine;
using TMPro;

public class TurnUI : MonoBehaviour
{
    public TurnManager turnManager;
    public TextMeshProUGUI turnText;

    private void Update()
    {
        if (turnManager == null || turnText == null)
            return;

        if (turnManager.IsPlayer1Turn())
        {
            turnText.text = "PLAYER 1 TURN";
        }
        else
        {
            turnText.text = "PLAYER 2 TURN";
        }
    }
}