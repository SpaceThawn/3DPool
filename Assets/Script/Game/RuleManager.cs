using UnityEngine;

public class RuleManager : MonoBehaviour
{
    public enum PlayerGroup
    {
        None,
        Solid,
        Stripe
    }

    [Header("Player Groups")]
    public PlayerGroup player1Group = PlayerGroup.None;
    public PlayerGroup player2Group = PlayerGroup.None;

    public PlayerGroup GetPlayerGroup(
        TurnManager.PlayerTurn player
    )
    {
        if (player == TurnManager.PlayerTurn.Player1)
            return player1Group;

        return player2Group;
    }

    public void SetPlayerGroup(
        TurnManager.PlayerTurn player,
        PlayerGroup group
    )
    {
        if (player == TurnManager.PlayerTurn.Player1)
        {
            player1Group = group;
        }
        else
        {
            player2Group = group;
        }

        Debug.Log(
            player +
            " = " +
            group
        );
    }
}