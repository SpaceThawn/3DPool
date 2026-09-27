using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum PlayerTurn
    {
        Player1,
        Player2
    }

    [Header("Current Turn")]
    public PlayerTurn currentTurn =
        PlayerTurn.Player1;

    private void Start()
    {
        Debug.Log(
            "Lượt hiện tại: " +
            currentTurn
        );
    }

    public void SwitchTurn()
    {
        if (currentTurn == PlayerTurn.Player1)
        {
            currentTurn = PlayerTurn.Player2;
        }
        else
        {
            currentTurn = PlayerTurn.Player1;
        }

        Debug.Log(
            "Chuyển lượt → " +
            currentTurn
        );
    }

    public bool IsPlayer1Turn()
    {
        return currentTurn ==
            PlayerTurn.Player1;
    }

    public bool IsPlayer2Turn()
    {
        return currentTurn ==
            PlayerTurn.Player2;
    }

    public bool IsMyTurn(PlayerTurn player)
    {
        return currentTurn == player;
    }
}