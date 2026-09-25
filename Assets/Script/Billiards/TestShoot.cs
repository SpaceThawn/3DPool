using UnityEngine;
using UnityEngine.InputSystem;

public class TestShoot : MonoBehaviour
{
    public BallPhysics ball;

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //ball.Shoot(Vector3.right, 3f);
            ball.Shoot(new Vector3(0.5f, 0f, 1f), 1f);
            //ball.Shoot(new Vector3(0f, 0f, 1f), 1f);
            //ball.Shoot(new Vector3(0f, 0f, -1f), 1f);
        }
    }
}
