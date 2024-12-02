using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoPointerZoneDetector : MonoBehaviour
{
    public static bool InsideRingLeft;
    public static bool InsideRingRight;




    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out TwoPointerZone zone))
        {
            if (zone.isLeftZone)
            {
                InsideRingLeft = true;
            }
            else
            {
                InsideRingRight = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out TwoPointerZone zone))
        {
            if (zone.isLeftZone)
            {
                InsideRingLeft = false;
            }
            else
            {
                InsideRingRight = false;
            }
        }
    }
}
