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

    private void LateUpdate()
    {
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
}