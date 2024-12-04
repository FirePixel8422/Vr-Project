using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class BowlingController : InteractableButton
{
    public Transform[] bowlingPawnSpawnpoints;
    public Pickupable bowlingPawnPrefab;

    private Vector3[] spawnPositions;
    private Pickupable[] bowlingPawns;

    public VibrationParamaters vibrationParams;



    public override void Start()
    {
        base.Start();

        spawnPositions = new Vector3[bowlingPawnSpawnpoints.Length];
        bowlingPawns = new Pickupable[bowlingPawnSpawnpoints.Length];

        for (int i = 0; i < bowlingPawnSpawnpoints.Length; i++)
        {
            spawnPositions[i] = bowlingPawnSpawnpoints[i].position;

            bowlingPawns[i] = Instantiate(bowlingPawnPrefab, spawnPositions[i], Quaternion.identity);

            Destroy(bowlingPawnSpawnpoints[i].gameObject);
        }
    }


    public override void Pickup(InteractionController hand)
    {
        base.Pickup(hand);

        ResetBowlingPawns();
    }


    private void ResetBowlingPawns()
    {
        for (int i = 0; i < bowlingPawns.Length; i++)
        {
            if (bowlingPawns[i].connectedHand != null)
            {
                bowlingPawns[i].connectedHand.isHoldingObject = false;
            }

            bowlingPawns[i].Drop();

            bowlingPawns[i].transform.SetPositionAndRotation(spawnPositions[i], Quaternion.identity);
        }

        Hand.Left.SendVibration(vibrationParams);
        Hand.Right.SendVibration(vibrationParams);
    }
}
