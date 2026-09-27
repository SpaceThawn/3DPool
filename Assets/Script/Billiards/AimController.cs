/*using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private void Update()
    {
        if (Mouse.current == null)
            return;

        float mouseX = Mouse.current.delta.ReadValue().x;

        transform.Rotate(
            Vector3.up,
            mouseX * rotationSpeed * Time.deltaTime
        );
    }
}*/

using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    [Header("Mouse Look")]
    public float rotationSpeed = 100f;

    [Header("Turn")]
    public TurnManager turnManager;
    public TurnManager.PlayerTurn playerTurn;

    private bool cursorLocked = false;

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        if (turnManager == null)
            return;

        if (!turnManager.IsMyTurn(playerTurn))
            return;

        if (Mouse.current == null)
            return;

        // ESC để mở chuột
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            UnlockCursor();
            return;
        }

        // Nếu cursor đang mở,
        // click vào Game để khóa lại
        if (!cursorLocked)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                LockCursor();
            }

            return;
        }

        // =====================================
        // MOUSE LOOK
        // =====================================

        // Khi đang giữ chuột để lấy lực
        // thì không xoay
        if (Mouse.current.leftButton.isPressed)
            return;

        float mouseX =
            Mouse.current.delta.ReadValue().x;

        transform.Rotate(
            Vector3.up,
            mouseX * rotationSpeed * Time.deltaTime
        );
    }

    private void LockCursor()
    {
        cursorLocked = true;

        Cursor.visible = false;

        Cursor.lockState =
            CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        cursorLocked = false;

        Cursor.visible = true;

        Cursor.lockState =
            CursorLockMode.None;
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        // Unity đôi khi tự nhả cursor khi
        // Game View mất focus rồi lấy lại.
        if (hasFocus && cursorLocked)
        {
            Cursor.visible = false;

            Cursor.lockState =
                CursorLockMode.Locked;
        }
    }
}