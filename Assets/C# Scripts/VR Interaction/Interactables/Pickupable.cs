using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;



public enum PickupRotationMode : byte
{
    Custom,
    SnapToHand,
    KeepWorldRotation,
}


[RequireComponent(typeof(Rigidbody))]
[BurstCompile]
public class Pickupable : Interactable
{

    public PickupRotationMode pickupRotationMode = PickupRotationMode.KeepWorldRotation;

    public float throwVelocityMultiplier = 1;

    [Header("Max velocity on each axis (direction is kept)")]
    public Vector3 velocityClamp = new Vector3(3, 3, 3);

    [Header("Release object with 0 velocity of released with less then minRequiredVelocity")]
    public float minRequiredVelocityXYZ = 0.065f;


    private Rigidbody rb;


    [BurstCompile]
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }



    [BurstCompile]
    public override void Pickup(InteractionController hand)
    {
        base.Pickup(hand);

        transform.SetParent(hand.heldItemHolder, false, pickupRotationMode == PickupRotationMode.KeepWorldRotation);
        rb.isKinematic = true;
    }


    [BurstCompile]
    public override void Throw(Vector3 velocity, Vector3 angularVelocity)
    {
        base.Throw(velocity, angularVelocity);

        transform.parent = null;

        rb.isKinematic = false;


        Vector3 targetVelocity = velocity * throwVelocityMultiplier;

        //only if velocity is MORE then minRequiredVelocityXYZ set rigidBody velocity to targetVelocity
        if (math.abs(targetVelocity.x) + math.abs(targetVelocity.y) + math.abs(targetVelocity.z) > minRequiredVelocityXYZ)
        {
            rb.angularVelocity = angularVelocity;


            // Calculate the radius vector from the center of mass to the point
            Vector3 radius = transform.position - rb.worldCenterOfMass;

            // Calculate the linear velocity caused by angular velocity
            Vector3 tangentialVelocity = Vector3.Cross(angularVelocity, radius);

            rb.velocity = VectorLogic.ClampDirection(targetVelocity + tangentialVelocity, velocityClamp);
        }
    }


    [BurstCompile]
    public override void Drop()
    {
        base.Drop();

        transform.parent = null;

        rb.isKinematic = false;
    }





    public bool debugRBCenterOfMass;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (debugRBCenterOfMass == false)
        {
            return;
        }

        
        if (TryGetComponent(out rb))
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.TransformPoint(rb.centerOfMass), 0.03f); // Visualize center of mass
        }
    }
}