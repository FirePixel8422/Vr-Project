using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;


[BurstCompile]
public class FragmentScalar : MonoBehaviour
{

    [BurstCompile]
    public IEnumerator SchrinkFragments(Rigidbody[] shatterPieces, float decayDelay, float decaySpeed)
    {
        yield return new WaitForSeconds(decayDelay);

        bool pieceFullyShrinked = false;

        while (true)
        {
            for (int i = 0; i < shatterPieces.Length; i++)
            {
                shatterPieces[i].transform.localScale = SchrinkScale(shatterPieces[i].transform.localScale, decaySpeed, Time.deltaTime, out pieceFullyShrinked);
            }

            if (pieceFullyShrinked == true)
            {
                Destroy(gameObject);
                yield break;
            }

            yield return null;
        }
    }

    [BurstCompile]
    private float3 SchrinkScale(float3 input, float decaySpeed, float deltaTime, out bool pieceFullyShrinked)
    {
        input.x -= decaySpeed * deltaTime;
        input.y -= decaySpeed * deltaTime;
        input.z -= decaySpeed * deltaTime;

        //all axis are equal, if X is 0, entire scale (XYZ) is 0
        if (input.x <= 0)
        {
            input = float3.zero;
            pieceFullyShrinked = true;
        }
        else
        {
            pieceFullyShrinked = false;
        }

        return input;
    }
}