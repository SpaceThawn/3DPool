using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("References")]
    public RuleManager ruleManager;
    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI reasonText;

    private void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (ruleManager == null)
            return;

        if (!ruleManager.gameOver)
            return;

        ShowGameOver();
    }

    private void ShowGameOver()
    {
        if (gameOverPanel == null)
            return;

        gameOverPanel.SetActive(true);

        if (winnerText == null ||
            reasonText == null)
        {
            return;
        }

        if (ruleManager.lastWinner ==
            TurnManager.PlayerTurn.Player1)
        {
            winnerText.text =
                "PLAYER 1 WINS";
        }
        else
        {
            winnerText.text =
                "PLAYER 2 WINS";
        }

        reasonText.text =
            ruleManager.gameOverReason;
    }
}