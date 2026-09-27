using UnityEngine;

public class CueBallController : MonoBehaviour
{
    [Header("Spawn")]
    public Transform spawnPoint;

    private Rigidbody rb;

    private bool isRespawning = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Respawn()
    {
        if (isRespawning)
            return;

        if (spawnPoint == null)
        {
            Debug.LogWarning(
                "CueBall chưa được gán Spawn Point!"
            );

            return;
        }

        isRespawning = true;

        // =====================================
        // RESET VỊ TRÍ KHI CUEBALL ĐANG INACTIVE
        // =====================================

        transform.position =
            spawnPoint.position;

        transform.rotation =
            spawnPoint.rotation;

        // =====================================
        // ACTIVE LẠI
        // =====================================

        gameObject.SetActive(true);

        // =====================================
        // RESET PHYSICS
        // =====================================

        if (rb != null)
        {
            rb.linearVelocity =
                Vector3.zero;

            rb.angularVelocity =
                Vector3.zero;

            rb.WakeUp();
        }

        // Đảm bảo Physics nhận vị trí mới
        Physics.SyncTransforms();

        isRespawning = false;

        Debug.Log(
            "CueBall đã respawn tại: " +
            transform.position
        );
    }
}