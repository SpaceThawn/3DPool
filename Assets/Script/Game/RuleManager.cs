using System.Collections.Generic;
using UnityEngine;

public class RuleManager : MonoBehaviour
{
    public enum PlayerGroup
    {
        None,
        Solid,
        Stripe
    }

    [Header("References")]
    public TurnManager turnManager;

    [Header("Player Groups")]
    public PlayerGroup player1Group = PlayerGroup.None;
    public PlayerGroup player2Group = PlayerGroup.None;

    [Header("All Balls")]
    public List<BallData> allBalls =
        new List<BallData>();

    [Header("Shot Result")]
    public bool continueTurn = false;
    public bool shotFoul = false;

    private BallData firstBallHit;

    [Header("Ball In Hand")]
    public bool ballInHand = true;

    public TurnManager.PlayerTurn ballInHandPlayer =
        TurnManager.PlayerTurn.Player1;

    [Header("Cue Ball")]
    public bool cueBallPlaced = true;

    [Header("Game State")]
    public bool gameOver = false;

    public TurnManager.PlayerTurn lastWinner;

    public string gameOverReason = "";

    private void Start()
    {
        ballInHand = true;

        ballInHandPlayer =
            TurnManager.PlayerTurn.Player1;

        cueBallPlaced = true;

        Debug.Log(
            "GAME START → PLAYER 1"
        );

        Debug.Log(
            "CueBall đã có vị trí → " +
            "có thể bắn hoặc giữ chuột phải để đặt lại."
        );
    }

    // =========================================================
    // PLAYER GROUP
    // =========================================================

    public PlayerGroup GetPlayerGroup(
        TurnManager.PlayerTurn player
    )
    {
        if (player ==
            TurnManager.PlayerTurn.Player1)
        {
            return player1Group;
        }

        return player2Group;
    }

    public void SetPlayerGroup(
        TurnManager.PlayerTurn player,
        PlayerGroup group
    )
    {
        if (player ==
            TurnManager.PlayerTurn.Player1)
        {
            player1Group = group;
        }
        else
        {
            player2Group = group;
        }

        Debug.Log(
            player + " = " + group
        );
    }

    public PlayerGroup GetOpponentGroup(
        PlayerGroup group
    )
    {
        if (group == PlayerGroup.Solid)
            return PlayerGroup.Stripe;

        if (group == PlayerGroup.Stripe)
            return PlayerGroup.Solid;

        return PlayerGroup.None;
    }

    public bool HasGroupAssigned()
    {
        return
            player1Group != PlayerGroup.None ||
            player2Group != PlayerGroup.None;
    }

    // =========================================================
    // SHOT
    // =========================================================

    public void BeginShot()
    {
        continueTurn = false;
        shotFoul = false;
        firstBallHit = null;
    }

    public bool ShouldContinueTurn()
    {
        return continueTurn;
    }

    // =========================================================
    // GROUP
    // =========================================================

    public void AssignGroupFromPocket(
        TurnManager.PlayerTurn player,
        BallData ballData
    )
    {
        if (ballData == null)
            return;

        if (ballData.IsEightBall())
            return;

        if (ballData.IsCueBall())
            return;

        if (GetPlayerGroup(player) !=
            PlayerGroup.None)
        {
            return;
        }

        PlayerGroup group;

        if (ballData.IsSolid())
        {
            group = PlayerGroup.Solid;
        }
        else if (ballData.IsStripe())
        {
            group = PlayerGroup.Stripe;
        }
        else
        {
            return;
        }

        PlayerGroup opponentGroup =
            GetOpponentGroup(group);

        SetPlayerGroup(
            player,
            group
        );

        TurnManager.PlayerTurn opponent =
            player ==
            TurnManager.PlayerTurn.Player1
                ? TurnManager.PlayerTurn.Player2
                : TurnManager.PlayerTurn.Player1;

        SetPlayerGroup(
            opponent,
            opponentGroup
        );

        continueTurn = true;

        Debug.Log(
            player +
            " chọn " +
            group +
            " → " +
            opponent +
            " = " +
            opponentGroup
        );
    }

    public bool IsCorrectGroup(
        TurnManager.PlayerTurn player,
        BallData ballData
    )
    {
        if (ballData == null)
            return false;

        PlayerGroup playerGroup =
            GetPlayerGroup(player);

        if (playerGroup ==
            PlayerGroup.None)
        {
            return false;
        }

        if (playerGroup ==
            PlayerGroup.Solid)
        {
            return ballData.IsSolid();
        }

        if (playerGroup ==
            PlayerGroup.Stripe)
        {
            return ballData.IsStripe();
        }

        return false;
    }

    // =========================================================
    // POCKET
    // =========================================================

    public void ProcessPocketedBall(
        TurnManager.PlayerTurn player,
        BallData ballData
    )
    {
        if (ballData == null)
            return;

        if (ballData.IsEightBall())
        {
            ProcessEightBall(player);
            return;
        }

        if (GetPlayerGroup(player) ==
            PlayerGroup.None)
        {
            AssignGroupFromPocket(
                player,
                ballData
            );

            return;
        }

        if (IsCorrectGroup(
            player,
            ballData))
        {
            continueTurn = true;

            Debug.Log(
                player +
                " pocket đúng nhóm: " +
                ballData.ballNumber +
                " → tiếp tục lượt."
            );
        }
        else
        {
            Debug.Log(
                player +
                " pocket sai nhóm: " +
                ballData.ballNumber +
                " → đổi lượt."
            );
        }
    }

    // =========================================================
    // EIGHT BALL
    // =========================================================

    public void ProcessEightBall(
        TurnManager.PlayerTurn player
    )
    {
        if (CanShootEightBall(player))
        {
            gameOver = true;

            lastWinner = player;

            gameOverReason =
                player +
                " pocketed the 8-ball correctly.";

            Debug.Log(
                "WINNER: " +
                player +
                " đã pocket 8-ball đúng luật!"
            );
        }
        else
        {
            gameOver = true;

            TurnManager.PlayerTurn opponent =
                player ==
                TurnManager.PlayerTurn.Player1
                    ? TurnManager.PlayerTurn.Player2
                    : TurnManager.PlayerTurn.Player1;

            lastWinner = opponent;

            gameOverReason =
                player +
                " pocketed the 8-ball too early.";

            Debug.Log(
                "LOSER: " +
                player +
                " pocket 8-ball quá sớm!"
            );
        }
    }

    public bool AreAllSolidsPocketed()
    {
        foreach (BallData ball in allBalls)
        {
            if (ball == null)
                continue;

            if (ball.IsSolid() &&
                !ball.isPocketed)
            {
                return false;
            }
        }

        return true;
    }

    public bool AreAllStripesPocketed()
    {
        foreach (BallData ball in allBalls)
        {
            if (ball == null)
                continue;

            if (ball.IsStripe() &&
                !ball.isPocketed)
            {
                return false;
            }
        }

        return true;
    }

    public bool CanShootEightBall(
        TurnManager.PlayerTurn player
    )
    {
        PlayerGroup group =
            GetPlayerGroup(player);

        if (group == PlayerGroup.None)
            return false;

        if (group == PlayerGroup.Solid)
            return AreAllSolidsPocketed();

        if (group == PlayerGroup.Stripe)
            return AreAllStripesPocketed();

        return false;
    }

    public bool IsEightBallPocketed()
    {
        foreach (BallData ball in allBalls)
        {
            if (ball == null)
                continue;

            if (ball.IsEightBall())
                return ball.isPocketed;
        }

        return false;
    }

    public void RegisterBall(
        BallData ball
    )
    {
        if (ball == null)
            return;

        if (!allBalls.Contains(ball))
        {
            allBalls.Add(ball);
        }
    }

    // =========================================================
    // CUE BALL PLACEMENT
    // =========================================================

    public bool IsBallInHand()
    {
        return ballInHand;
    }

    public bool IsCueBallPlaced()
    {
        return cueBallPlaced;
    }

    public void ConfirmBallPlacement()
    {
        cueBallPlaced = true;

        Debug.Log(
            "CueBall đã được đặt → " +
            "có thể bắn hoặc đặt lại."
        );
    }

    // =========================================================
    // SHOT START
    // =========================================================

    public void ConfirmShotStarted()
    {
        ballInHand = false;
        cueBallPlaced = false;

        Debug.Log(
            "SHOT STARTED → " +
            "Ball In Hand kết thúc."
        );
    }

    // =========================================================
    // BALL IN HAND
    // =========================================================

    public void BeginBallInHand()
    {
        ballInHand = true;

        ballInHandPlayer =
            turnManager.currentTurn;

        cueBallPlaced = false;

        Debug.Log(
            "BALL IN HAND → " +
            ballInHandPlayer
        );
    }

    // =========================================================
    // FOUL
    // =========================================================

    public void HandleFoul()
    {
        shotFoul = true;

        continueTurn = false;

        Debug.Log(
            "FOUL → Mất lượt + Ball in Hand."
        );
    }

    public bool IsShotFoul()
    {
        return shotFoul;
    }

    // =========================================================
    // FIRST BALL HIT
    // =========================================================

    public void RegisterCueBallHit(
        BallData ballData
    )
    {
        if (ballData == null)
            return;

        if (firstBallHit != null)
            return;

        firstBallHit = ballData;

        if (turnManager == null)
            return;

        TurnManager.PlayerTurn player =
            turnManager.currentTurn;

        Debug.Log(
            "CUE BALL HIT FIRST → Ball " +
            ballData.ballNumber
        );

        if (ballData.IsEightBall())
        {
            Debug.Log(
                "Cue Ball chạm 8-ball."
            );

            return;
        }

        if (GetPlayerGroup(player) ==
            PlayerGroup.None)
        {
            Debug.Log(
                "Chưa xác định Solid/Stripe " +
                "→ không kiểm tra foul nhóm."
            );

            return;
        }

        if (!IsCorrectGroup(
            player,
            ballData))
        {
            HandleFoul();

            Debug.Log(
                "FOUL! " +
                player +
                " đánh trúng sai nhóm trước."
            );
        }
        else
        {
            Debug.Log(
                "First hit hợp lệ → " +
                player
            );
        }
    }

    // =========================================================
    // CHUYỂN LƯỢT
    // =========================================================

    public void PrepareBallInHandForNextPlayer()
    {
        if (turnManager == null)
            return;

        turnManager.SwitchTurn();

        BeginBallInHand();

        Debug.Log(
            "BALL IN HAND → " +
            turnManager.currentTurn
        );
    }
}