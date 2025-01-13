using UnityEngine;




public class BowlingController : InteractableButton
{
    public Transform[] bowlingPawnSpawnpoints;
    public Pickupable bowlingPawnPrefab;

    private Vector3[] spawnPositions;
    private Pickupable[] bowlingPawns;

    public VibrationParamaters vibrationParams;



    protected override void Start()
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
            bowlingPawns[i].rb.velocity = Vector3.zero;
            bowlingPawns[i].rb.angularVelocity = Vector3.zero;
        }

        Hand.Left.SendVibration(vibrationParams);
        Hand.Right.SendVibration(vibrationParams);
    }
}
