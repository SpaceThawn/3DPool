using UnityEngine;

public class BallRack : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform rackPoint;

    private void Start()
    {
        CreateRack();
    }

    private void CreateRack()
    {
        float spacing = 0.185f;

        for (int row = 0; row < 5; row++)
        {
            int ballsInRow = row + 1;

            for (int col = 0; col < ballsInRow; col++)
            {
                // Rack mở rộng về phía bên trái
                float x =
                    -row *
                    spacing *
                    Mathf.Sqrt(3f) / 2f;

                // Các ball trong cùng hàng trải theo Z
                float z =
                    (col - row * 0.5f) *
                    spacing;

                Vector3 position =
                    rackPoint.position +
                    new Vector3(x, 0f, z);

                GameObject ball =
                    Instantiate(
                        ballPrefab,
                        position,
                        Quaternion.identity
                    );

                ball.name =
                    "Ball_" + (row + 1) + "_" + (col + 1);
            }
        }
    }
}