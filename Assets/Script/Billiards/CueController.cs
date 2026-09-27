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

    private bool cueVisible = true;

    private TurnManager.PlayerTurn lastTurn;

    [Header("Turn")]
    public TurnManager turnManager;
    public TurnManager.PlayerTurn playerTurn;

    private void Start()
    {
        if (turnManager != null)
        {
            lastTurn = turnManager.currentTurn;

            if (!turnManager.IsMyTurn(playerTurn))
            {
                HideCue();
            }
        }
    }

    private void Update()
    {
        if (turnManager == null)
            return;

        // ========================================
        // TURN CHANGED
        // ========================================

        if (lastTurn != turnManager.currentTurn)
        {
            lastTurn = turnManager.currentTurn;

            if (turnManager.IsMyTurn(playerTurn))
            {
                ShowCue();
            }
            else
            {
                HideCue();
            }
        }

        // Không phải lượt của mình
        if (!turnManager.IsMyTurn(playerTurn))
            return;

        if (ball == null)
            return;

        if (transform.parent == null)
            return;

        if (!cueVisible)
            return;

        if (Mouse.current == null)
            return;

        // ================================
        // POWER
        // ================================

        if (Mouse.current.leftButton.isPressed)
        {
            HandlePower();
        }

        // ================================
        // CUE POSITION
        // ================================

        UpdateCuePosition();
    }

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

    // ========================================
    // HIDE
    // ========================================

    public void HideCue()
    {
        cueVisible = false;

        power = 0f;

        if (cueVisual != null)
        {
            cueVisual.SetActive(false);
        }
    }

    // ========================================
    // SHOW
    // ========================================

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

    // ========================================
    // POSITION
    // ========================================

    private void UpdateCuePosition()
    {
        if (ball == null)
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

        if (directionToBall.sqrMagnitude < 0.001f)
            return;

        directionToBall.Normalize();

        // Kéo cơ về phía Player
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

        // Cơ luôn hướng về bi
        Vector3 lookDirection =
            ball.position -
            transform.position;

        lookDirection.y = 0f;

        if (lookDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation =
                Quaternion.LookRotation(
                    lookDirection
                );
        }
    }
}
