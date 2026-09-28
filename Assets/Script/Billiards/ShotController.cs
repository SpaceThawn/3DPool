/*using UnityEngine;
using UnityEngine.InputSystem;

public class ShotController : MonoBehaviour
{
    public BallPhysics ball;

    public float minForce = 1f;
    public float maxForce = 8f;

    private float currentForce;
    private bool charging;

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            charging = true;
            currentForce = minForce;
        }

        if (charging && Mouse.current.leftButton.isPressed)
        {
            currentForce += 5f * Time.deltaTime;

            currentForce = Mathf.Clamp(
                currentForce,
                minForce,
                maxForce
            );
        }

        if (charging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Shoot();

            charging = false;
        }
    }

    private void Shoot()
    {
        Vector3 direction = ball.transform.position - transform.position;

        direction.y = 0f;
        direction.Normalize();

        ball.Shoot(direction, currentForce);
    }
}*/

/*using UnityEngine;
using UnityEngine.InputSystem;

public class ShotController : MonoBehaviour
{
    public BallPhysics ball;

    public float minForce = 1f;
    public float maxForce = 8f;

    public PowerBarUI powerBar;

    private float currentForce;
    private bool charging;

    private void Start()
    {
        if (powerBar != null)
        {
            powerBar.SetPower(0f);
        }
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            charging = true;
            currentForce = minForce;
        }

        if (charging && Mouse.current.leftButton.isPressed)
        {
            currentForce += 5f * Time.deltaTime;

            currentForce = Mathf.Clamp(
                currentForce,
                minForce,
                maxForce
            );

            UpdatePowerUI();
        }

        if (charging && Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Shoot();

            charging = false;

            if (powerBar != null)
            {
                powerBar.SetPower(0f);
            }
        }
    }

    private void UpdatePowerUI()
    {
        if (powerBar == null)
            return;

        float normalizedPower =
            Mathf.InverseLerp(
                minForce,
                maxForce,
                currentForce
            );

        powerBar.SetPower(normalizedPower);
    }

    private void Shoot()
    {
        Vector3 direction =
            ball.transform.position - transform.position;

        direction.y = 0f;
        direction.Normalize();

        ball.Shoot(direction, currentForce);
    }
}*/

/*using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotController : MonoBehaviour
{
    public BallPhysics ball;
    public CueController cue;

    [Header("Force")]
    public float minForce = 0f;
    public float maxForce = 5f;

    [Header("UI")]
    public PowerBarUI powerBar;

    [Header("Ball Stop")]
    public float stopVelocity = 0.05f;
    public float stopAngularVelocity = 0.05f;
    public float stopDelay = 0.15f;

    [Header("Turn")]
    public TurnManager turnManager;
    public TurnManager.PlayerTurn playerTurn;

    [Header("Cue Ball")]
    public CueBallController cueBallController;

    private bool waitingForBallToStop = false;

    private void Update()
    {
        if (turnManager == null)
            return;

        if (!turnManager.IsMyTurn(playerTurn))
            return;

        if (Mouse.current == null)
            return;

        if (ball == null || cue == null)
            return;

        // Đang chờ bi
        if (waitingForBallToStop)
            return;

        // ================================
        // GIỮ CHUỘT
        // ================================

        if (Mouse.current.leftButton.isPressed)
        {
            UpdatePowerUI();
        }

        // ================================
        // THẢ CHUỘT
        // ================================

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Shoot();
        }
    }

    private void UpdatePowerUI()
    {
        float power = cue.GetPowerPercent();

        if (powerBar != null)
        {
            powerBar.SetPower(power);
        }
    }

    private void Shoot()
    {
        float power = cue.GetPowerPercent();

        Debug.Log(
            "Release mouse - Power = "
            + (power * 100f)
            + "%"
        );

        if (power <= 0.001f)
        {
            Debug.Log("Power = 0% → Không bắn.");
            return;
        }

        float force =
            Mathf.Lerp(
                minForce,
                maxForce,
                power
            );

        // =====================================
        // HƯỚNG
        // =====================================

        Vector3 direction = cue.transform.forward;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            Debug.Log("Direction không hợp lệ.");
            return;
        }

        direction.Normalize();

        // =====================================
        // THỬ BẮN
        // =====================================

        bool shotSuccessful =
            ball.Shoot(
                direction,
                force
            );

        // =====================================
        // BẮN THẤT BẠI
        // =====================================

        if (!shotSuccessful)
        {
            Debug.Log(
                "Shoot thất bại - Ball chưa stopped."
            );

            return;
        }

        // =====================================
        // TỚI ĐÂY MỚI ẨN CUE
        // =====================================

        cue.HideCue();

        // =====================================
        // RESET POWER
        // =====================================

        cue.ResetPower();

        if (powerBar != null)
        {
            powerBar.SetPower(0f);
        }

        // =====================================
        // CHỜ BI DỪNG
        // =====================================

        StartCoroutine(
            WaitForBallToStop()
        );
    }

    *//*private IEnumerator WaitForBallToStop()
    {
        waitingForBallToStop = true;

        // Chờ physics xử lý cú đánh
        yield return new WaitForFixedUpdate();

        while (true)
        {
            if (ball == null)
            {
                waitingForBallToStop = false;
                yield break;
            }

            // Ball bị pocket
            if (!ball.gameObject.activeInHierarchy)
            {
                break;
            }

            if (ball.IsStopped())
            {
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(
            stopDelay
        );

        waitingForBallToStop = false;

        // Chỉ hiện nếu Cue vẫn tồn tại
        if (cue != null)
        {
            cue.ShowCue();
        }
    }*//*

    private IEnumerator WaitForBallToStop()
    {
        waitingForBallToStop = true;

        // Chờ physics xử lý cú đánh
        yield return new WaitForFixedUpdate();

        while (true)
        {
            bool allBallsStopped = true;

            GameObject[] balls =
                GameObject.FindGameObjectsWithTag("Ball");

            foreach (GameObject ballObject in balls)
            {
                if (!ballObject.activeInHierarchy)
                    continue;

                BallPhysics ballPhysics =
                    ballObject.GetComponent<BallPhysics>();

                if (ballPhysics == null)
                    continue;

                if (!ballPhysics.IsStopped())
                {
                    allBallsStopped = false;
                    break;
                }
            }

            if (allBallsStopped)
            {
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        // Chờ thêm một chút
        yield return new WaitForSeconds(stopDelay);

        // =====================================
        // KIỂM TRA CUE BALL
        // =====================================

        if (cueBallController != null &&
            !cueBallController.gameObject.activeInHierarchy)
        {
            Debug.Log(
                "CueBall bị pocket → Respawn."
            );

            cueBallController.Respawn();
        }

        // =====================================
        // HIỆN CUE
        // =====================================

        waitingForBallToStop = false;

        if (cue != null)
        {
            cue.ShowCue();
        }

        // =====================================
        // ĐỔI LƯỢT
        // =====================================

        if (turnManager != null)
        {
            turnManager.SwitchTurn();
        }

        Debug.Log(
            "Tất cả bi đã dừng → Đổi lượt."
        );
    }
}
*/

using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShotController : MonoBehaviour
{
    public BallPhysics ball;
    public CueController cue;

    [Header("Force")]
    public float minForce = 0f;
    public float maxForce = 5f;

    [Header("UI")]
    public PowerBarUI powerBar;

    [Header("Ball Stop")]
    public float stopVelocity = 0.05f;
    public float stopAngularVelocity = 0.05f;
    public float stopDelay = 0.15f;

    [Header("Turn")]
    public TurnManager turnManager;
    public TurnManager.PlayerTurn playerTurn;

    [Header("Rule")]
    public RuleManager ruleManager;

    private bool waitingForBallToStop = false;

    public bool IsShotInProgress()
    {
        return waitingForBallToStop;
    }

    private void Update()
    {
        if (turnManager == null)
            return;

        if (ruleManager == null)
            return;

        if (ruleManager.gameOver)
            return;

        if (!turnManager.IsMyTurn(playerTurn))
            return;

        if (Mouse.current == null)
            return;

        // =====================================================
        // RIGHT đang được giữ → không xử lý chuột LEFT
        // =====================================================

        if (Mouse.current.rightButton.isPressed)
            return;

        if (ball == null ||
            cue == null)
        {
            return;
        }

        if (waitingForBallToStop)
            return;

        // =========================================
        // BALL IN HAND
        // =========================================
        //
        // Chưa đặt CueBall:
        // không được bắn.
        //
        // Đã đặt CueBall:
        // được bắn.
        // =========================================

        if (ruleManager.IsBallInHand() &&
            !ruleManager.IsCueBallPlaced())
        {
            return;
        }

        // =========================================
        // NHẤN CHUỘT TRÁI
        // =========================================

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            cue.ShowCue();
        }

        // =========================================
        // GIỮ CHUỘT TRÁI
        // =========================================

        if (Mouse.current.leftButton.isPressed)
        {
            UpdatePowerUI();
        }

        // =========================================
        // THẢ CHUỘT TRÁI
        // =========================================

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Shoot();
        }
    }

    private void UpdatePowerUI()
    {
        float power =
            cue.GetPowerPercent();

        if (powerBar != null)
        {
            powerBar.SetPower(power);
        }
    }

    private void Shoot()
    {
        if (waitingForBallToStop)
            return;

        float power =
            cue.GetPowerPercent();

        Debug.Log(
            "Release mouse - Power = " +
            (power * 100f) +
            "%"
        );

        if (power <= 0.001f)
        {
            Debug.Log(
                "Power = 0% → Không bắn."
            );

            cue.HideCue();

            return;
        }

        float force =
            Mathf.Lerp(
                minForce,
                maxForce,
                power
            );

        Vector3 direction =
            cue.transform.forward;

        direction.y = 0f;

        if (direction.sqrMagnitude <
            0.001f)
        {
            Debug.Log(
                "Direction không hợp lệ."
            );

            cue.HideCue();

            return;
        }

        direction.Normalize();

        bool shotSuccessful =
            ball.Shoot(
                direction,
                force
            );

        if (!shotSuccessful)
        {
            Debug.Log(
                "Shoot thất bại - Ball chưa stopped."
            );

            cue.HideCue();

            return;
        }

        // =========================================
        // SHOT BẮT ĐẦU
        // =========================================

        ruleManager.BeginShot();

        ruleManager.ConfirmShotStarted();

        cue.HideCue();
        cue.ResetPower();

        if (powerBar != null)
        {
            powerBar.SetPower(0f);
        }

        StartCoroutine(
            WaitForBallToStop()
        );
    }

    private IEnumerator WaitForBallToStop()
    {
        waitingForBallToStop = true;

        yield return new WaitForFixedUpdate();

        while (true)
        {
            bool allBallsStopped = true;

            GameObject[] balls =
                GameObject.FindGameObjectsWithTag(
                    "Ball"
                );

            foreach (GameObject ballObject in balls)
            {
                if (!ballObject.activeInHierarchy)
                    continue;

                BallPhysics ballPhysics =
                    ballObject.GetComponent<BallPhysics>();

                if (ballPhysics == null)
                    continue;

                if (!ballPhysics.IsStopped())
                {
                    allBallsStopped = false;
                    break;
                }
            }

            if (allBallsStopped)
                break;

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(
            stopDelay
        );

        if (ruleManager.gameOver)
        {
            Debug.Log(
                "GAME OVER → Không đổi lượt."
            );

            waitingForBallToStop = false;

            yield break;
        }

        if (ruleManager.IsShotFoul())
        {
            Debug.Log(
                "FOUL → Đổi lượt + Ball in Hand."
            );

            ruleManager
                .PrepareBallInHandForNextPlayer();

            waitingForBallToStop = false;

            yield break;
        }

        if (ruleManager.ShouldContinueTurn())
        {
            Debug.Log(
                turnManager.currentTurn +
                " tiếp tục lượt."
            );
        }
        else
        {
            turnManager.SwitchTurn();

            Debug.Log(
                "Không có pocket hợp lệ → Đổi lượt."
            );
        }

        waitingForBallToStop = false;

        // Không ShowCue ở đây.
        // Cue chỉ xuất hiện khi người chơi
        // nhấn giữ chuột trái.
    }
}