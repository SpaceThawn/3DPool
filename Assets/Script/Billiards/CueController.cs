/*using UnityEngine;

public class CueController : MonoBehaviour
{
    public Transform ball;

    public float distance = 1.8f;
    public float height = 1.2f;

    private void Update()
    {
        if (ball == null)
            return;

        Vector3 direction = transform.position - ball.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        direction.Normalize();

        transform.position =
            ball.position + direction * distance;

        transform.position =
            new Vector3(
                transform.position.x,
                height,
                transform.position.z
            );

        transform.LookAt(ball.position);
    }
}*/

using UnityEngine;
using UnityEngine.InputSystem;

public class CueController : MonoBehaviour
{
    public Transform ball;

    [Header("Cue Visual")]
    public GameObject cueVisual;

    [Header("Cue Position")]
    [Range(0f, 1f)]
    public float cuePositionPercent = 0.75f;

    public float height = 1.2f;

    [Header("Pull Back")]
    public float maxPullBack = 0.6f;

    [Header("Power")]
    public float powerSensitivity = 0.01f;

    private float power = 0f;

    private bool cueVisible = false;

    private TurnManager.PlayerTurn lastTurn;

    [Header("Turn")]
    public TurnManager turnManager;

    public TurnManager.PlayerTurn playerTurn;

    private ShotController shotController;

    private void Start()
    {
        HideCue();

        shotController = GetComponentInParent<ShotController>();

        if (turnManager != null)
        {
            lastTurn = turnManager.currentTurn;
        }
    }

    private void Update()
    {
        if (turnManager == null)
            return;

        // =========================================
        // ĐỔI LƯỢT
        // =========================================

        if (lastTurn != turnManager.currentTurn)
        {
            lastTurn =
                turnManager.currentTurn;

            // Không tự hiện Cue.
            HideCue();
        }

        if (!turnManager.IsMyTurn(playerTurn))
            return;

        if (ball == null)
            return;

        if (transform.parent == null)
            return;

        if (Mouse.current == null)
            return;

        // =====================================================
        // RIGHT đang được giữ → không xử lý LEFT
        // =====================================================

        if (Mouse.current.rightButton.isPressed)
            return;

        RuleManager ruleManager =
            FindAnyObjectByType<RuleManager>();

        if (ruleManager != null)
        {
            // =====================================
            // ĐANG BALL IN HAND
            // NHƯNG CHƯA ĐẶT BI
            // =====================================

            if (ruleManager.IsBallInHand() &&
                !ruleManager.IsCueBallPlaced())
            {
                HideCue();
                return;
            }
        }

        // =====================================================
        // Shot đang diễn ra → không được hiện lại Cue
        // =====================================================

        if (shotController != null &&
            shotController.IsShotInProgress())
        {
            HideCue();
            return;
        }

        // =========================================
        // NHẤN CHUỘT TRÁI
        // =========================================

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            ShowCue();
        }

        // =========================================
        // GIỮ CHUỘT TRÁI
        // =========================================

        if (Mouse.current.leftButton.isPressed)
        {
            if (!cueVisible)
            {
                ShowCue();
            }

            HandlePower();
        }

        // =========================================
        // CHỈ CẬP NHẬT CƠ KHI ĐANG HIỆN
        // =========================================

        if (cueVisible)
        {
            UpdateCuePosition();
        }
    }

    // =========================================================
    // POWER
    // =========================================================

    private void HandlePower()
    {
        float mouseY =
            Mouse.current.delta.ReadValue().y;

        power -=
            mouseY * powerSensitivity;

        power =
            Mathf.Clamp01(power);
    }

    public float GetPowerPercent()
    {
        return power;
    }

    public void ResetPower()
    {
        power = 0f;
    }

    // =========================================================
    // HIDE CUE
    // =========================================================

    public void HideCue()
    {
        cueVisible = false;

        power = 0f;

        if (cueVisual != null)
        {
            cueVisual.SetActive(false);
        }
    }

    // =========================================================
    // SHOW CUE
    // =========================================================

    public void ShowCue()
    {
        cueVisible = true;

        if (cueVisual != null)
        {
            cueVisual.SetActive(true);
        }

        UpdateCuePosition();
    }

    public bool IsCueVisible()
    {
        return cueVisible;
    }

    public void RefreshCuePosition()
    {
        if (!cueVisible)
            return;

        UpdateCuePosition();
    }

    // =========================================================
    // CUE POSITION
    // =========================================================

    private void UpdateCuePosition()
    {
        if (ball == null)
            return;

        if (transform.parent == null)
            return;

        Transform player =
            transform.parent;

        Vector3 playerPosition =
            player.position;

        Vector3 ballPosition =
            ball.position;

        Vector3 basePosition =
            Vector3.Lerp(
                playerPosition,
                ballPosition,
                cuePositionPercent
            );

        Vector3 directionToBall =
            ballPosition -
            basePosition;

        directionToBall.y = 0f;

        if (directionToBall.sqrMagnitude <
            0.001f)
        {
            return;
        }

        directionToBall.Normalize();

        Vector3 pullBack =
            -directionToBall *
            (power * maxPullBack);

        Vector3 finalPosition =
            basePosition +
            pullBack;

        finalPosition.y =
            height;

        transform.position =
            finalPosition;

        Vector3 lookDirection =
            ball.position -
            transform.position;

        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude >
            0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(
                    lookDirection
                );
        }
    }
}