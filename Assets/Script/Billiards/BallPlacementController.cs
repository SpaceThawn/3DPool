/*using UnityEngine;
using UnityEngine.InputSystem;

public class BallPlacementController : MonoBehaviour
{
    [Header("References")]
    public CueBallController cueBallController;
    public TurnManager turnManager;
    public RuleManager ruleManager;

    [Header("Table")]
    public LayerMask tableLayer;

    [Header("Placement")]
    public float ballRadius = 0.09f;

    [Header("Cue Ball Height")]
    public float placedBallHeight = 1.19f;
    public float floatingHeight = 1.8f;

    [Header("Table Bounds")]
    public float tableMinX = -3.91f;
    public float tableMaxX = 3.91f;
    public float tableMinZ = -1.91f;
    public float tableMaxZ = 1.91f;

    private Camera mainCamera;

    private Rigidbody cueBallRb;
    private Collider cueBallCollider;

    private bool isPlacementHeld = false;

    private Vector3 dragOffset;

    [Header("Mouse Drag")]
    public float dragSensitivity = 0.015f;

    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        mainCamera = Camera.main;

        if (cueBallController == null)
        {
            Debug.LogError(
                "BallPlacementController: Chưa gán CueBallController!"
            );

            enabled = false;
            return;
        }

        if (turnManager == null)
        {
            Debug.LogError(
                "BallPlacementController: Chưa gán TurnManager!"
            );

            enabled = false;
            return;
        }

        if (ruleManager == null)
        {
            Debug.LogError(
                "BallPlacementController: Chưa gán RuleManager!"
            );

            enabled = false;
            return;
        }

        cueBallRb =
            cueBallController.GetComponent<Rigidbody>();

        cueBallCollider =
            cueBallController.GetComponent<Collider>();

        if (cueBallRb == null)
        {
            Debug.LogError(
                "CueBall không có Rigidbody!"
            );

            enabled = false;
            return;
        }

        if (cueBallCollider == null)
        {
            Debug.LogError(
                "CueBall không có Collider!"
            );

            enabled = false;
            return;
        }


        // =====================================================
        // CUE BALL BAN ĐẦU
        // =====================================================

        cueBallController.gameObject.SetActive(true);

        cueBallCollider.enabled = true;

        cueBallRb.isKinematic = false;
        cueBallRb.useGravity = true;

        cueBallRb.linearVelocity = Vector3.zero;
        cueBallRb.angularVelocity = Vector3.zero;


        Vector3 position =
            cueBallController.transform.position;

        position.y =
            placedBallHeight;


        // Dùng transform.position khi khởi tạo
        // để chắc chắn vị trí Y đúng.
        cueBallController.transform.position =
            position;


        Physics.SyncTransforms();

        cueBallRb.WakeUp();

        isPlacementHeld = false;

        HideAllCues();

        Debug.Log(
            "GAME START → CueBall đã sẵn sàng. Cue đang ẩn."
        );
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (ruleManager == null)
            return;

        if (turnManager == null)
            return;

        if (cueBallController == null)
            return;

        if (ruleManager.gameOver)
            return;

        if (!ruleManager.IsBallInHand())
            return;

        if (!turnManager.IsMyTurn(
            ruleManager.ballInHandPlayer))
        {
            return;
        }


        // =====================================================
        // ĐANG CẦM CUE BALL
        // =====================================================

        if (isPlacementHeld)
        {
            if (Mouse.current.rightButton.isPressed)
            {
                UpdateFloatingBallPosition();
            }

            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                ConfirmPlacement();
            }

            return;
        }


        // =====================================================
        // NHẤN CHUỘT PHẢI
        //
        // Nếu LEFT đang được giữ → không xử lý RIGHT.
        // =====================================================

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (Mouse.current.leftButton.isPressed)
                return;

            BeginPlacement();
        }
    }


    // =========================================================
    // BẮT ĐẦU CẦM CUE BALL
    // =========================================================

    private void BeginPlacement()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return;


        // =====================================================
        // LẤY VỊ TRÍ CHUỘT TRÊN MẶT PHẲNG BÀN
        // =====================================================

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            mainCamera.ScreenPointToRay(
                mousePosition
            );

        Plane tablePlane =
            new Plane(
                Vector3.up,
                new Vector3(
                    0f,
                    placedBallHeight,
                    0f
                )
            );

        if (!tablePlane.Raycast(
            ray,
            out float distance))
        {
            return;
        }

        Vector3 mousePoint =
            ray.GetPoint(distance);


        // =====================================================
        // TÍNH OFFSET X/Z
        // =====================================================

        Vector3 ballPosition =
            cueBallController.transform.position;

        Vector3 ballPositionXZ =
            new Vector3(
                ballPosition.x,
                0f,
                ballPosition.z
            );

        Vector3 mousePointXZ =
            new Vector3(
                mousePoint.x,
                0f,
                mousePoint.z
            );

        dragOffset =
            ballPositionXZ -
            mousePointXZ;


        // =====================================================
        // BẮT ĐẦU CẦM
        // =====================================================

        isPlacementHeld = true;

        HideAllCues();


        // =====================================================
        // TẮT PHYSICS
        // =====================================================

        cueBallRb.linearVelocity =
            Vector3.zero;

        cueBallRb.angularVelocity =
            Vector3.zero;

        cueBallRb.isKinematic =
            true;

        cueBallRb.useGravity =
            false;


        // =====================================================
        // TẮT COLLIDER
        // =====================================================

        cueBallCollider.enabled =
            false;


        // =====================================================
        // NHẤC BALL LÊN
        // =====================================================

        Vector3 floatingPosition =
            cueBallController.transform.position;

        floatingPosition.y =
            floatingHeight;

        cueBallController.transform.position =
            floatingPosition;

        Physics.SyncTransforms();


        Debug.Log(
            "GIỮ CHUỘT PHẢI → Cầm CueBall lên." +
            "\nPosition = " +
            cueBallController.transform.position
        );
    }


    // =========================================================
    // DI CHUYỂN CUE BALL
    // =========================================================

    private void UpdateFloatingBallPosition()
    {
        if (!isPlacementHeld)
            return;

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return;

        // =====================================================
        // LẤY ĐỘ DI CHUYỂN CỦA CHUỘT
        // =====================================================

        Vector2 mouseDelta =
            Mouse.current.delta.ReadValue();

        if (mouseDelta.sqrMagnitude <= 0.001f)
            return;


        // =====================================================
        // HƯỚNG NGANG CỦA CAMERA
        // =====================================================

        Vector3 cameraRight =
            mainCamera.transform.right;

        cameraRight.y = 0f;

        if (cameraRight.sqrMagnitude > 0.001f)
        {
            cameraRight.Normalize();
        }


        // =====================================================
        // HƯỚNG DỌC MÀN HÌNH CỦA CAMERA
        //
        // Chỉ lấy X/Z.
        // Không cho Ball bay lên/xuống theo camera.
        // =====================================================

        Vector3 cameraForward =
            mainCamera.transform.forward;

        cameraForward.y = 0f;

        if (cameraForward.sqrMagnitude > 0.001f)
        {
            cameraForward.Normalize();
        }


        // =====================================================
        // CHUYỂN MOUSE DELTA → DI CHUYỂN TRÊN MẶT BÀN
        // =====================================================

        Vector3 movement =
            cameraRight * mouseDelta.x +
            cameraForward * mouseDelta.y;

        movement *= dragSensitivity;


        // =====================================================
        // VỊ TRÍ HIỆN TẠI
        // =====================================================

        Vector3 targetPosition =
            cueBallController.transform.position;


        // =====================================================
        // DI CHUYỂN X/Z
        // =====================================================

        targetPosition += movement;


        // =====================================================
        // GIỚI HẠN TRONG BÀN
        // =====================================================

        targetPosition.x =
            Mathf.Clamp(
                targetPosition.x,
                tableMinX,
                tableMaxX
            );

        targetPosition.z =
            Mathf.Clamp(
                targetPosition.z,
                tableMinZ,
                tableMaxZ
            );


        // =====================================================
        // KHI ĐANG CẦM → LUÔN BAY Ở Y = 1.8
        // =====================================================

        targetPosition.y =
            floatingHeight;


        // =====================================================
        // ĐẶT VỊ TRÍ
        // =====================================================

        cueBallController.transform.position =
            targetPosition;

        Physics.SyncTransforms();


        Debug.Log(
            "Đang kéo CueBall → " +
            cueBallController.transform.position
        );
    }


    // =========================================================
    // THẢ CHUỘT PHẢI
    // =========================================================

    private void ConfirmPlacement()
    {
        // =====================================================
        // KIỂM TRA VỊ TRÍ TRƯỚC
        // =====================================================

        if (!IsValidPosition())
        {
            Debug.Log(
                "Vị trí CueBall không hợp lệ → tiếp tục kéo."
            );

            return;
        }


        // =====================================================
        // LẤY VỊ TRÍ X/Z HIỆN TẠI
        // =====================================================

        Vector3 position =
            cueBallController.transform.position;


        // =====================================================
        // QUAN TRỌNG:
        // THẢ XUỐNG MẶT BÀN
        // =====================================================

        position.y =
            placedBallHeight;


        Debug.Log(
            "Đặt CueBall xuống → " +
            "Y = " +
            placedBallHeight
        );


        // =====================================================
        // SET TRANSFORM TRỰC TIẾP
        //
        // Không dùng Rigidbody.position ở đây.
        // Vì Rigidbody đang có Freeze Position Y.
        // =====================================================

        cueBallController.transform.position =
            position;

        Physics.SyncTransforms();


        // =====================================================
        // BẬT COLLIDER
        // =====================================================

        cueBallCollider.enabled =
            true;


        // =====================================================
        // BẬT PHYSICS
        // =====================================================

        cueBallRb.isKinematic =
            false;

        cueBallRb.useGravity =
            true;

        cueBallRb.linearVelocity =
            Vector3.zero;

        cueBallRb.angularVelocity =
            Vector3.zero;


        Physics.SyncTransforms();

        cueBallRb.WakeUp();


        // =====================================================
        // KẾT THÚC CẦM
        // =====================================================

        isPlacementHeld = false;


        // =====================================================
        // RULE
        // =====================================================

        ruleManager.ConfirmBallPlacement();


        // =====================================================
        // CUE VẪN ẨN
        // =====================================================

        HideAllCues();


        Debug.Log(
            "THẢ CHUỘT PHẢI → CueBall đã đặt xuống bàn." +
            "\nPosition = " +
            cueBallController.transform.position
        );
    }


    // =========================================================
    // KIỂM TRA VỊ TRÍ
    // =========================================================

    private bool IsValidPosition()
    {
        Vector3 center =
            cueBallController.transform.position;

        Vector3 checkPosition =
            new Vector3(
                center.x,
                placedBallHeight,
                center.z
            );


        Collider[] colliders =
            Physics.OverlapSphere(
                checkPosition,
                ballRadius
            );


        foreach (Collider collider in colliders)
        {
            BallData ballData =
                collider.GetComponentInParent<BallData>();

            if (ballData == null)
                continue;


            if (ballData.gameObject ==
                cueBallController.gameObject)
            {
                continue;
            }


            return false;
        }


        return true;
    }


    // =========================================================
    // GET CUE CONTROLLER
    // =========================================================

    private CueController GetCueController(
        string playerName
    )
    {
        GameObject player =
            GameObject.Find(playerName);

        if (player == null)
            return null;

        Transform cueTransform =
            player.transform.Find("Cue");

        if (cueTransform == null)
            return null;

        return cueTransform.GetComponent<CueController>();
    }


    // =========================================================
    // HIDE ALL CUES
    // =========================================================

    private void HideAllCues()
    {
        CueController player1Cue =
            GetCueController("Player");

        CueController player2Cue =
            GetCueController("Player2");


        if (player1Cue != null)
        {
            player1Cue.HideCue();
        }


        if (player2Cue != null)
        {
            player2Cue.HideCue();
        }
    }
}*/


using UnityEngine;
using UnityEngine.InputSystem;

public class BallPlacementController : MonoBehaviour
{
    [Header("References")]
    public CueBallController cueBallController;
    public TurnManager turnManager;
    public RuleManager ruleManager;

    [Header("Placement")]
    public float ballRadius = 0.09f;

    [Header("Cue Ball Height")]
    public float placedBallHeight = 1.19f;
    public float floatingHeight = 1.8f;

    [Header("Table Bounds")]
    public float tableMinX = -3.91f;
    public float tableMaxX = 3.91f;
    public float tableMinZ = -1.91f;
    public float tableMaxZ = 1.91f;

    [Header("Break Zone")]
    [Tooltip("Bật nếu vùng break nằm ở nửa X âm.")]
    public bool breakZoneOnNegativeX = false;

    [Tooltip("Đường phân chia vùng break theo trục X.")]
    public float breakLineX = 0f;

    [Header("Mouse Drag")]
    public float dragSensitivity = 0.015f;

    private Camera mainCamera;
    private Rigidbody cueBallRb;
    private Collider cueBallCollider;

    private bool isPlacementHeld;

    // Giữ giới hạn cho đến khi cú đánh break thành công.
    private bool isBreakPlacement = true;

    private void Start()
    {
        mainCamera = Camera.main;

        if (cueBallController == null ||
            turnManager == null ||
            ruleManager == null)
        {
            Debug.LogError(
                "BallPlacementController: Chưa gán đủ References."
            );

            enabled = false;
            return;
        }

        cueBallRb =
            cueBallController.GetComponent<Rigidbody>();

        cueBallCollider =
            cueBallController.GetComponent<Collider>();

        if (cueBallRb == null || cueBallCollider == null)
        {
            Debug.LogError(
                "CueBall cần có Rigidbody và Collider."
            );

            enabled = false;
            return;
        }

        cueBallController.gameObject.SetActive(true);

        cueBallCollider.enabled = true;
        cueBallRb.isKinematic = false;
        cueBallRb.useGravity = true;

        cueBallRb.linearVelocity = Vector3.zero;
        cueBallRb.angularVelocity = Vector3.zero;

        Vector3 position =
            cueBallController.transform.position;

        position.y = placedBallHeight;

        cueBallController.transform.position = position;

        Physics.SyncTransforms();
        cueBallRb.WakeUp();

        isPlacementHeld = false;
        isBreakPlacement = true;

        HideAllCues();

        Debug.Log(
            "BREAK PLACEMENT ACTIVE" +
            " | Negative X = " + breakZoneOnNegativeX +
            " | Break line X = " + breakLineX
        );
    }

    private void Update()
    {
        if (Mouse.current == null ||
            ruleManager == null ||
            turnManager == null ||
            cueBallController == null)
        {
            return;
        }

        if (ruleManager.gameOver)
            return;

        if (!ruleManager.IsBallInHand())
            return;

        if (!turnManager.IsMyTurn(
            ruleManager.ballInHandPlayer))
        {
            return;
        }

        if (isPlacementHeld)
        {
            if (Mouse.current.rightButton.isPressed)
                UpdateFloatingBallPosition();

            if (Mouse.current.rightButton.wasReleasedThisFrame)
                ConfirmPlacement();

            return;
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (Mouse.current.leftButton.isPressed)
                return;

            BeginPlacement();
        }
    }

    private void BeginPlacement()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return;

        isPlacementHeld = true;

        HideAllCues();

        cueBallRb.linearVelocity = Vector3.zero;
        cueBallRb.angularVelocity = Vector3.zero;

        cueBallRb.isKinematic = true;
        cueBallRb.useGravity = false;

        cueBallCollider.enabled = false;

        Vector3 position =
            cueBallController.transform.position;

        position.y = floatingHeight;

        cueBallController.transform.position = position;

        Physics.SyncTransforms();

        Debug.Log(
            "Đang cầm CueBall." +
            " | Break Placement = " + isBreakPlacement
        );
    }

    private void UpdateFloatingBallPosition()
    {
        if (!isPlacementHeld)
            return;

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return;

        Vector2 mouseDelta =
            Mouse.current.delta.ReadValue();

        if (mouseDelta.sqrMagnitude <= 0.001f)
            return;

        Vector3 cameraRight =
            mainCamera.transform.right;

        cameraRight.y = 0f;

        if (cameraRight.sqrMagnitude > 0.001f)
            cameraRight.Normalize();

        Vector3 cameraForward =
            mainCamera.transform.forward;

        cameraForward.y = 0f;

        if (cameraForward.sqrMagnitude > 0.001f)
            cameraForward.Normalize();

        Vector3 movement =
            (cameraRight * mouseDelta.x +
             cameraForward * mouseDelta.y) *
            dragSensitivity;

        Vector3 current =
            cueBallController.transform.position;

        current.y = placedBallHeight;

        // Thử di chuyển X.
        Vector3 candidateX = current;
        candidateX.x += movement.x;

        ClampToAllowedArea(ref candidateX);

        if (IsValidPosition(candidateX))
            current.x = candidateX.x;

        // Thử di chuyển Z.
        Vector3 candidateZ = current;
        candidateZ.z += movement.z;

        ClampToAllowedArea(ref candidateZ);

        if (IsValidPosition(candidateZ))
            current.z = candidateZ.z;

        current.y = floatingHeight;

        cueBallController.transform.position = current;

        Physics.SyncTransforms();
    }

    private void ClampToAllowedArea(ref Vector3 position)
    {
        position.x = Mathf.Clamp(
            position.x,
            tableMinX,
            tableMaxX
        );

        position.z = Mathf.Clamp(
            position.z,
            tableMinZ,
            tableMaxZ
        );

        if (!isBreakPlacement)
            return;

        if (breakZoneOnNegativeX)
        {
            position.x = Mathf.Min(
                position.x,
                breakLineX - ballRadius
            );
        }
        else
        {
            position.x = Mathf.Max(
                position.x,
                breakLineX + ballRadius
            );
        }
    }

    private bool IsValidPosition(Vector3 position)
    {
        position.y = placedBallHeight;

        if (position.x < tableMinX ||
            position.x > tableMaxX ||
            position.z < tableMinZ ||
            position.z > tableMaxZ)
        {
            return false;
        }

        // Kiểm tra vùng break.
        if (isBreakPlacement)
        {
            if (breakZoneOnNegativeX &&
                position.x > breakLineX - ballRadius)
            {
                return false;
            }

            if (!breakZoneOnNegativeX &&
                position.x < breakLineX + ballRadius)
            {
                return false;
            }
        }

        Collider[] colliders = Physics.OverlapSphere(
            position,
            ballRadius
        );

        foreach (Collider hit in colliders)
        {
            if (hit == null || !hit.enabled)
                continue;

            if (hit.transform == cueBallController.transform ||
                hit.transform.IsChildOf(
                    cueBallController.transform))
            {
                continue;
            }

            BallData ballData =
                hit.GetComponentInParent<BallData>();

            if (ballData != null &&
                ballData.gameObject !=
                cueBallController.gameObject)
            {
                return false;
            }

            if (IsRailOrCushion(hit.transform))
                return false;
        }

        return true;
    }

    private bool IsRailOrCushion(Transform target)
    {
        Transform current = target;

        while (current != null)
        {
            string objectName = current.name;

            if (objectName.StartsWith("Rail_") ||
                objectName.StartsWith("Cushion_"))
            {
                return true;
            }

            current = current.parent;
        }

        return false;
    }

    private void ConfirmPlacement()
    {
        Vector3 position =
            cueBallController.transform.position;

        position.y = placedBallHeight;

        if (!IsValidPosition(position))
        {
            Debug.LogWarning(
                "Vị trí CueBall không hợp lệ. " +
                "Tiếp tục kéo để chọn vị trí khác."
            );

            return;
        }

        cueBallController.transform.position = position;

        Physics.SyncTransforms();

        cueBallCollider.enabled = true;

        cueBallRb.isKinematic = false;
        cueBallRb.useGravity = true;

        cueBallRb.linearVelocity = Vector3.zero;
        cueBallRb.angularVelocity = Vector3.zero;

        Physics.SyncTransforms();
        cueBallRb.WakeUp();

        isPlacementHeld = false;

        // KHÔNG tắt isBreakPlacement tại đây.
        // Người chơi có thể nhấc lên và đặt lại trước khi bắn.

        ruleManager.ConfirmBallPlacement();

        HideAllCues();

        Debug.Log(
            "CueBall placed at " +
            cueBallController.transform.position +
            " | Break Placement vẫn còn: " +
            isBreakPlacement
        );
    }

    // Được ShotController gọi khi cú đánh đầu tiên thành công.
    public void CompleteBreakPlacement()
    {
        if (!isBreakPlacement)
            return;

        isBreakPlacement = false;

        Debug.Log(
            "BREAK SHOT đã bắt đầu thành công. " +
            "Đã bỏ giới hạn vùng break."
        );
    }

    private CueController GetCueController(string playerName)
    {
        GameObject player = GameObject.Find(playerName);

        if (player == null)
            return null;

        Transform cueTransform =
            player.transform.Find("Cue");

        if (cueTransform == null)
            return null;

        return cueTransform.GetComponent<CueController>();
    }

    private void HideAllCues()
    {
        CueController player1Cue =
            GetCueController("Player");

        CueController player2Cue =
            GetCueController("Player2");

        if (player1Cue != null)
            player1Cue.HideCue();

        if (player2Cue != null)
            player2Cue.HideCue();
    }
}