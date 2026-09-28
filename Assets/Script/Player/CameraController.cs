/*using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float distance = 6f;
    public float height = 3f;
    public float mouseSensitivity = 0.2f;

    private float rotationX = 15f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        rotationY += mouseDelta.x * mouseSensitivity;
        rotationX -= mouseDelta.y * mouseSensitivity;

        rotationX = Mathf.Clamp(rotationX, -20f, 60f);

        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);

        Vector3 offset = rotation * new Vector3(0f, 0f, -distance);

        transform.position = target.position + Vector3.up * height + offset;

        transform.LookAt(target.position + Vector3.up * 1f);
    }
}*//*

using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float distance = 4f;
    public float height = 2.5f;
    public float sensitivity = 0.2f;

    private float yaw;
    private float pitch = 15f;

    [Header("Turn")]
    public TurnManager turnManager;
    public Transform player1CameraTarget;
    public Transform player2CameraTarget;

    private void LateUpdate()
    {
        if (turnManager != null)
        {
            if (turnManager.IsPlayer1Turn())
            {
                target = player1CameraTarget;
            }
            else
            {
                target = player2CameraTarget;
            }
        }

        if (target == null)
            return;

        bool aiming = false;

        if (Mouse.current != null)
        {
            aiming =
                Mouse.current.leftButton.isPressed;
        }

        // Chỉ xoay camera khi KHÔNG giữ chuột trái
        if (!aiming && Mouse.current != null)
        {
            Vector2 mouseDelta =
                Mouse.current.delta.ReadValue();

            yaw += mouseDelta.x * sensitivity;
            pitch -= mouseDelta.y * sensitivity;

            pitch = Mathf.Clamp(
                pitch,
                -10f,
                60f
            );
        }

        Quaternion rotation =
            Quaternion.Euler(
                pitch,
                yaw,
                0f
            );

        Vector3 direction =
            rotation * Vector3.back;

        transform.position =
            target.position +
            direction * distance +
            Vector3.up * height;

        transform.LookAt(
            target.position
        );
    }
}*/

using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float distance = 4f;
    public float height = 2.5f;
    public float sensitivity = 0.2f;

    private float yaw;
    private float pitch = 15f;

    [Header("Turn")]
    public TurnManager turnManager;
    public Transform player1CameraTarget;
    public Transform player2CameraTarget;

    private void LateUpdate()
    {
        // =========================================
        // CHỌN CAMERA TARGET THEO LƯỢT
        // =========================================

        if (turnManager != null)
        {
            if (turnManager.IsPlayer1Turn())
            {
                target =
                    player1CameraTarget;
            }
            else
            {
                target =
                    player2CameraTarget;
            }
        }

        if (target == null)
            return;

        // =========================================
        // KIỂM TRA CHUỘT
        // =========================================

        if (Mouse.current == null)
            return;

        bool leftPressed =
            Mouse.current.leftButton.isPressed;

        bool rightPressed =
            Mouse.current.rightButton.isPressed;

        // =========================================
        // XOAY CAMERA
        // =========================================
        //
        // Không giữ chuột phải:
        //     Camera có thể xoay.
        //
        // Giữ chuột phải:
        //     Camera đứng yên để đặt CueBall.
        //
        // Khi giữ chuột trái:
        //     Camera cũng đứng yên để kéo cơ ổn định.
        //

        bool canRotateCamera =
            !rightPressed &&
            !leftPressed;

        if (canRotateCamera)
        {
            Vector2 mouseDelta =
                Mouse.current.delta.ReadValue();

            yaw +=
                mouseDelta.x *
                sensitivity;

            pitch -=
                mouseDelta.y *
                sensitivity;

            pitch =
                Mathf.Clamp(
                    pitch,
                    -10f,
                    60f
                );
        }

        // =========================================
        // TÍNH CAMERA ROTATION
        // =========================================

        Quaternion rotation =
            Quaternion.Euler(
                pitch,
                yaw,
                0f
            );

        Vector3 direction =
            rotation *
            Vector3.back;

        // =========================================
        // CAMERA POSITION
        // =========================================

        transform.position =
            target.position +
            direction * distance +
            Vector3.up * height;

        // =========================================
        // CAMERA LOOK AT
        // =========================================

        transform.LookAt(
            target.position
        );
    }
}