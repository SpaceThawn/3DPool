using UnityEngine;

public class BallData : MonoBehaviour
{
    [Header("Ball Info")]
    public BallType ballType;

    [Range(0, 15)]
    public int ballNumber;

    public bool IsCueBall()
    {
        return ballType == BallType.CueBall;
    }

    public bool IsSolid()
    {
        return ballType == BallType.Solid;
    }

    public bool IsStripe()
    {
        return ballType == BallType.Stripe;
    }

    public bool IsEightBall()
    {
        return ballType == BallType.EightBall;
    }
}