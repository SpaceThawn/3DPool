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

    private bool waitingForBallToStop = false;

    private void Update()
    {
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

    private IEnumerator WaitForBallToStop()
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
    }
}
