using System;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;


[Flags]
public enum TurnConstraints : byte
{
    X = 1,
    Y = 2,
    Z = 4,
}

[BurstCompile]
public class TurnInteractable : Interactable, IInteractableUpdater
{
    [Header("Highest parent of this interactable")]
    public Transform objectRoot;

    [Header("What axises can this interactable turn?")]
    public TurnConstraints turnOnAxis;

    public bool snapPlayerHandToTransform;

    public float interactionRange;

    //public Transform DEBUG_HAND;

    public Vector3 rotOffset;
    public Vector3 rotClampMin, rotClampMax;



    private void Start()
    {
        InteractableUpdateManager.AddUpdater(this);
    }




    public bool requireUpdate => heldByPlayer;


    [BurstCompile]
    public void OnUpdate()
    {
        Vector3 transformPos = transform.position;
        Vector3 handTransformPos = connectedHand.transform.position;

        if (snapPlayerHandToTransform)
        {
            SnapHandToTransform();
        }


        RotateTransform(transformPos, handTransformPos);
    }

    [BurstCompile]
    private void SnapHandToTransform()
    {
        float handDistanceToTransform = Vector3.Distance(transformPos, handTransformPos);

        if (handDistanceToTransform > interactionRange)
        {
            connectedHand.isHoldingObject = false;
            connectedHand = null;
            heldByPlayer = false;

            return;
        }
    }


    [BurstCompile]
    private void RotateTransform(Vector3 transformPos, Vector3 handTransformPos)
    {
        Vector3 dir = (handTransformPos - transformPos).normalized;

        if (turnOnAxis.HasFlag(TurnConstraints.X))
        {
            dir.x = 0;
        }
        if (turnOnAxis.HasFlag(TurnConstraints.Y))
        {
            dir.y = 0;
        }
        if (turnOnAxis.HasFlag(TurnConstraints.Z))
        {
            dir.z = 0;
        }


        Debug.DrawLine(transformPos, transformPos + dir);

        // Calculate quaternion for the local direction
        Vector3 eulerRotation = (Quaternion.LookRotation(dir) * Quaternion.Euler(rotOffset - objectRoot.eulerAngles)).eulerAngles;

        eulerRotation.x = math.clamp(NormalizeAngle(eulerRotation.x), rotClampMin.x, rotClampMax.x);
        eulerRotation.y = math.clamp(NormalizeAngle(eulerRotation.y), rotClampMin.y, rotClampMax.y);
        eulerRotation.z = math.clamp(NormalizeAngle(eulerRotation.z), rotClampMin.z, rotClampMax.z);

        transform.localEulerAngles = eulerRotation;
    }


    // Normalize angle to [-180, 180] range
    [BurstCompile]
    private float NormalizeAngle(float angle)
    {
        return (angle + 180f) % 360f - 180f;
    }




    [BurstCompile]
    protected override void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one * 0.1f);
    }
}
