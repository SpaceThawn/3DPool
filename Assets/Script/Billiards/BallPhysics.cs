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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.linearDamping = 0.5f;
        rb.angularDamping = 0.05f;

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
