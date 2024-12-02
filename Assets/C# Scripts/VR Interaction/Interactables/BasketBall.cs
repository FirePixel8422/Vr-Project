using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : Pickupable
{
    protected override void OnThrow(Vector3 velocity, Vector3 angularVelocity)
    {
        base.OnThrow(velocity, angularVelocity);

        BasketBallTrigger ball = GetComponentInChildren<BasketBallTrigger>();
        if (ball != null)
        {
            ball.insideRingLeft = TwoPointerZoneDetector.InsideRingLeft;
            ball.insideRingRight = TwoPointerZoneDetector.InsideRingRight;
        }
    }
}
