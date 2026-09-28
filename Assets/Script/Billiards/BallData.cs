using UnityEngine;

public class BallData : MonoBehaviour
{
    [Header("Ball Info")]
    public BallType ballType;

    [Range(0, 15)]
    public int ballNumber;

    [Header("State")]
    public bool isPocketed = false;

    public bool IsCueBall() => ballType == BallType.CueBall;

    public bool IsSolid() => ballType == BallType.Solid;

    public bool IsStripe() => ballType == BallType.Stripe;

    public bool IsEightBall() => ballType == BallType.EightBall;
}