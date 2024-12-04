using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : Pickupable
{
    public override void Throw(Vector3 velocity, Vector3 angularVelocity)
    {
        base.Throw(velocity, angularVelocity);

        BasketBallTrigger ball = GetComponentInChildren<BasketBallTrigger>();
        if (ball != null)
        {
            ball.insideRingLeft = TwoPointerZoneDetector.InsideRingLeft;
            ball.insideRingRight = TwoPointerZoneDetector.InsideRingRight;
        }
    }
}
