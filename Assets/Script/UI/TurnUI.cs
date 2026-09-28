/*using UnityEngine;
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
}*/

using UnityEngine;
using TMPro;

public class TurnUI : MonoBehaviour
{
    [Header("References")]
    public TurnManager turnManager;
    public RuleManager ruleManager;

    public TextMeshProUGUI turnText;

    private void Update()
    {
        if (turnManager == null ||
            ruleManager == null ||
            turnText == null)
        {
            return;
        }

        // ========================================
        // GAME OVER
        // ========================================

        if (ruleManager.gameOver)
        {
            turnText.text =
                "GAME OVER";

            return;
        }

        // ========================================
        // BALL IN HAND
        // ========================================

        if (ruleManager.IsBallInHand())
        {
            string ballInHandPlayer;

            if (ruleManager.ballInHandPlayer ==
                TurnManager.PlayerTurn.Player1)
            {
                ballInHandPlayer =
                    "PLAYER 1";
            }
            else
            {
                ballInHandPlayer =
                    "PLAYER 2";
            }

            turnText.text =
                ballInHandPlayer +
                " - BALL IN HAND" +
                "\n\n" +
                "PLACE CUE BALL";

            return;
        }

        // ========================================
        // CURRENT TURN
        // ========================================

        string currentTurnText;

        if (turnManager.currentTurn ==
            TurnManager.PlayerTurn.Player1)
        {
            currentTurnText =
                "PLAYER 1 TURN";
        }
        else
        {
            currentTurnText =
                "PLAYER 2 TURN";
        }

        // ========================================
        // PLAYER 1 GROUP
        // ========================================

        string player1Group =
            GetGroupText(
                ruleManager.player1Group
            );

        // ========================================
        // PLAYER 2 GROUP
        // ========================================

        string player2Group =
            GetGroupText(
                ruleManager.player2Group
            );

        // ========================================
        // UI
        // ========================================

        turnText.text =
            currentTurnText +
            "\n\n" +
            "P1: " +
            player1Group +
            "    P2: " +
            player2Group;
    }

    private string GetGroupText(
        RuleManager.PlayerGroup group
    )
    {
        if (group ==
            RuleManager.PlayerGroup.Solid)
        {
            return "SOLID";
        }

        if (group ==
            RuleManager.PlayerGroup.Stripe)
        {
            return "STRIPE";
        }

        return "OPEN";
    }
}