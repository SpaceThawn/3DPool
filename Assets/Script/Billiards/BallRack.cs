/*using UnityEngine;

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
}*/

using UnityEngine;

public class BallRack : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform rackPoint;

    // Thứ tự bi trong rack 8-ball
    private int[] rackNumbers =
    {
        1,

        10, 2,

        11, 8, 3,

        6, 4, 12, 14,

        7, 13, 5, 15, 9
    };

    private void Start()
    {
        CreateRack();
    }

    private void CreateRack()
    {
        float spacing = 0.185f;

        int ballIndex = 0;

        for (int row = 0; row < 5; row++)
        {
            int ballsInRow = row + 1;

            for (int col = 0; col < ballsInRow; col++)
            {
                float x =
                    -row *
                    spacing *
                    Mathf.Sqrt(3f) / 2f;

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

                ball.name = "Ball_" + rackNumbers[ballIndex];

                // Lấy dữ liệu của quả bi
                BallData ballData = ball.GetComponent<BallData>();

                if (ballData != null)
                {
                    int number =
                        rackNumbers[ballIndex];

                    ballData.ballNumber = number;

                    if (number == 8)
                    {
                        ballData.ballType =
                            BallType.EightBall;
                    }
                    else if (number >= 1 && number <= 7)
                    {
                        ballData.ballType =
                            BallType.Solid;
                    }
                    else
                    {
                        ballData.ballType =
                            BallType.Stripe;
                    }
                }

                BallVisual ballVisual = ball.GetComponent<BallVisual>();

                if (ballVisual != null)
                {
                    ballVisual.ApplyVisual();
                }

                ballIndex++;
            }
        }
    }
}