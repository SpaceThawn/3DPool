/*using UnityEngine;

public class Pocket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Ball vào pocket: " + other.name);

            Rigidbody rb = other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            other.gameObject.SetActive(false);
        }
    }
}*/

/*using UnityEngine;

public class Pocket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        Debug.Log(
            "Ball vào pocket: " + other.name
        );

        Rigidbody rb =
            other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.Sleep();
        }

        other.gameObject.SetActive(false);
    }
}*/

/*using UnityEngine;

public class Pocket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        Debug.Log(
            "Ball vào pocket: " +
            other.name
        );

        // ================================
        // CUE BALL
        // ================================

        CueBallController cueBall =
            other.GetComponent<CueBallController>();

        if (cueBall != null)
        {
            Rigidbody rb =
                other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity =
                    Vector3.zero;

                rb.angularVelocity =
                    Vector3.zero;

                rb.Sleep();
            }

            other.gameObject.SetActive(false);

            Debug.Log(
                "SCRATCH! CueBall đã vào pocket."
            );

            return;
        }

        // ================================
        // BI THƯỜNG
        // ================================

        Rigidbody ballRb =
            other.GetComponent<Rigidbody>();

        if (ballRb != null)
        {
            ballRb.linearVelocity =
                Vector3.zero;

            ballRb.angularVelocity =
                Vector3.zero;

            ballRb.Sleep();
        }

        other.gameObject.SetActive(false);

        Debug.Log(
            "Bi thường đã được pocket: " +
            other.name
        );
    }
}*/

/*using UnityEngine;

public class Pocket : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        Debug.Log(
            "Ball vào pocket: " +
            other.name
        );

        // =====================================
        // CUE BALL
        // =====================================

        CueBallController cueBall =
            other.GetComponent<CueBallController>();

        if (cueBall != null)
        {
            Rigidbody rb =
                other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity =
                    Vector3.zero;

                rb.angularVelocity =
                    Vector3.zero;

                rb.Sleep();
            }

            other.gameObject.SetActive(false);

            Debug.Log(
                "SCRATCH! CueBall đã vào pocket."
            );

            return;
        }

        // =====================================
        // BI THƯỜNG
        // =====================================

        Rigidbody ballRb =
            other.GetComponent<Rigidbody>();

        if (ballRb != null)
        {
            ballRb.linearVelocity =
                Vector3.zero;

            ballRb.angularVelocity =
                Vector3.zero;

            ballRb.Sleep();
        }

        other.gameObject.SetActive(false);

        Debug.Log(
            "Bi thường đã được pocket: " +
            other.name
        );
    }
}*/

using UnityEngine;

public class Pocket : MonoBehaviour
{
    [Header("Game")]
    public TurnManager turnManager;
    public RuleManager ruleManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ball"))
            return;

        Debug.Log(
            "Ball vào pocket: " +
            other.name
        );

        BallData ballData =
            other.GetComponent<BallData>();

        // ========================================
        // CUE BALL
        // ========================================

        CueBallController cueBall =
            other.GetComponent<CueBallController>();

        if (cueBall != null)
        {
            Rigidbody rb =
                other.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity =
                    Vector3.zero;

                rb.angularVelocity =
                    Vector3.zero;

                rb.Sleep();
            }

            // =====================================================
            // CUE BALL VÀO POCKET
            // =====================================================

            if (ruleManager != null)
            {
                ruleManager.HandleFoul();
            }

            Debug.Log(
                "SCRATCH! CueBall đã vào pocket."
            );

            // Respawn CueBall
            cueBall.Respawn();

            return;
        }

        // ========================================
        // NORMAL BALL
        // ========================================

        if (ballData != null)
        {
            ballData.isPocketed = true;

            Debug.Log(
                "Ball " +
                ballData.ballNumber +
                " → isPocketed = true"
            );

            if (turnManager != null &&
                ruleManager != null)
            {
                ruleManager.ProcessPocketedBall(
                    turnManager.currentTurn,
                    ballData
                );
            }
        }

        Rigidbody ballRb =
            other.GetComponent<Rigidbody>();

        if (ballRb != null)
        {
            ballRb.linearVelocity =
                Vector3.zero;

            ballRb.angularVelocity =
                Vector3.zero;

            ballRb.Sleep();
        }

        other.gameObject.SetActive(false);

        Debug.Log(
            "Bi thường đã được pocket: " +
            other.name
        );
    }
}