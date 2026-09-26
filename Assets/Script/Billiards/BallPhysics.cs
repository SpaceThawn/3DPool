/*using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 direction, float force)
    {
        direction.y = 0f;
        direction.Normalize();

        rb.AddForce(direction * force, ForceMode.Impulse);
    }
}
*/

using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Ball Speed")]
    public float speedMultiplier = 2.5f;

    [Header("Stopping")]
    public float stopVelocity = 0.05f;
    public float stopAngularVelocity = 0.05f;

    [Header("Runtime Velocity")]
    [SerializeField] private Vector3 linearVelocity;
    [SerializeField] private Vector3 angularVelocity;

    private void Update()
    {
        linearVelocity = rb.linearVelocity;
        angularVelocity = rb.angularVelocity;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.05f;

        rb.maxAngularVelocity = 150f;

        rb.collisionDetectionMode =
            CollisionDetectionMode.ContinuousDynamic;

        rb.interpolation =
            RigidbodyInterpolation.Interpolate;

        // Khóa bi không được di chuyển theo trục Y
        rb.constraints =
            RigidbodyConstraints.FreezePositionY;
    }


    public bool Shoot(Vector3 direction, float force)
    {
        if (rb == null)
            return false;

        if (!IsStopped())
            return false;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return false;

        direction.Normalize();

        rb.WakeUp();

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        float finalForce =
            force * speedMultiplier;

        rb.AddForce(
            direction * finalForce,
            ForceMode.Impulse
        );

        // ================================
        // TẠO CHUYỂN ĐỘNG LĂN
        // ================================

        float radius = 0.09f;

        // Với ForceMode.Impulse:
        //
        // Impulse = Mass × ΔVelocity
        //
        // Mass của Ball = 1
        // nên vận tốc tăng thêm ≈ finalForce
        float linearSpeed =
            finalForce / rb.mass;

        // Trục xoay vuông góc với hướng chuyển động
        Vector3 rollingAxis =
            Vector3.Cross(
                Vector3.up,
                direction
            ).normalized;

        // Điều kiện lăn không trượt:
        // angularSpeed = linearSpeed / radius
        float angularSpeed =
            linearSpeed / radius;

        // Giới hạn theo maxAngularVelocity
        angularSpeed =
            Mathf.Min(
                angularSpeed,
                rb.maxAngularVelocity
            );

        rb.angularVelocity =
            rollingAxis * angularSpeed;

        Debug.Log(
            "ROLL TEST" +
            "\nDirection = " + direction +
            "\nRolling Axis = " + rollingAxis +
            "\nLinear Speed = " + linearSpeed +
            "\nAngular Speed = " + angularSpeed +
            "\nAngular Velocity = " + rb.angularVelocity
        );

        return true;
    }


    /*public bool IsStopped()
    {
        if (rb == null)
            return true;

        return
            rb.linearVelocity.sqrMagnitude
                < stopVelocity * stopVelocity
            &&
            rb.angularVelocity.sqrMagnitude
                < stopAngularVelocity * stopAngularVelocity;
    }*/

    public bool IsStopped()
    {
        if (rb == null)
            return true;

        return rb.linearVelocity.sqrMagnitude < stopVelocity * stopVelocity;
    }

    private void FixedUpdate()
    {
        if (rb == null)
            return;

        Vector3 velocity = rb.linearVelocity;

        // Bỏ vận tốc Y vì bi chỉ chạy trên mặt bàn
        velocity.y = 0f;

        if (velocity.sqrMagnitude < 0.001f)
        {
            return;
        }

        float speed = velocity.magnitude;

        float radius = 0.09f;

        // Hướng lăn vuông góc với hướng chuyển động
        Vector3 rollingAxis =
            Vector3.Cross(
                Vector3.up,
                velocity.normalized
            ).normalized;

        float angularSpeed =
            speed / radius;

        angularSpeed =
            Mathf.Min(
                angularSpeed,
                rb.maxAngularVelocity
            );

        rb.angularVelocity =
            rollingAxis * angularSpeed;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(
            "\n========== BALL COLLISION ==========" +
            "\nObject: " + collision.gameObject.name +
            "\nContacts: " + collision.contactCount +
            "\nRelative Velocity: " + collision.relativeVelocity +
            "\nRB Velocity: " + rb.linearVelocity
        );

        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint c = collision.GetContact(i);

            Debug.Log(
                "Contact " + i +
                " | Point: " + c.point +
                " | Normal: " + c.normal +
                " | Separation: " + c.separation
            );
        }

        Debug.Log("====================================");
    }*/

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody otherRb = collision.rigidbody;

        Debug.Log(
            "\n========== BALL COLLISION ==========" +
            "\nThis: " + gameObject.name +
            "\nOther: " + collision.gameObject.name +
            "\nThis Velocity: " + rb.linearVelocity +
            "\nThis Speed: " + rb.linearVelocity.magnitude +
            "\nOther Velocity: " +
                (otherRb != null
                    ? otherRb.linearVelocity.ToString()
                    : "NO RIGIDBODY") +
            "\nOther Speed: " +
                (otherRb != null
                    ? otherRb.linearVelocity.magnitude.ToString()
                    : "NO RIGIDBODY") +
            "\nRelative Velocity: " + collision.relativeVelocity +
            "\nImpulse: " + collision.impulse +
            "\nContacts: " + collision.contactCount
        );

        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactPoint c = collision.GetContact(i);

            Debug.Log(
                "Contact " + i +
                " | Point: " + c.point +
                " | Normal: " + c.normal +
                " | Separation: " + c.separation
            );
        }

        Debug.Log("====================================");
    }
}
