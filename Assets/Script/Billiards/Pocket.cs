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

using UnityEngine;

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
}
