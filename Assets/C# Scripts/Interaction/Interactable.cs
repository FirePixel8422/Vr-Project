using System;
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
public class Interactable : MonoBehaviour
{
    private InteractionController connectedHand;


    public bool interactable = true;
    public bool isThrowable = true;
    public bool heldByPlayer;

    public PickupRotationMode pickupRotationMode = PickupRotationMode.KeepWorldRotation;

    public float throwVelocityMultiplier = 1;

    [Header("Max velocity on each axis (direction is kept)")]
    public Vector3 velocityClamp = new Vector3(3, 3, 3);

    [Header("Release object with 0 velocity of released with less then minRequiredVelocity")]
    public float minRequiredVelocityXYZ = 0.065f;


    public float objectSize;


    private Rigidbody rb;


    [BurstCompile]
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }



    #region Select And Deselect

    [BurstCompile]
    public virtual void Select()
    {

    }

    [BurstCompile]
    public virtual void DeSelect()
    {

    }

    #endregion




    [BurstCompile]
    public virtual void Pickup(InteractionController hand)
    {
        if (connectedHand != null)
        {
            connectedHand.isHoldingObject = false;
        }

        connectedHand = hand;
        heldByPlayer = true;

        transform.SetParent(hand.heldItemHolder, false, pickupRotationMode == PickupRotationMode.KeepWorldRotation);
        rb.isKinematic = true;
    }


    [BurstCompile]
    public virtual void Throw(Vector3 velocity, Vector3 angularVelocity)
    {
        connectedHand = null;
        heldByPlayer = false;

        transform.parent = null;
        connectedHand = null;

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
    public virtual void Drop()
    {
        connectedHand = null;
        heldByPlayer = false;

        transform.parent = null;
        connectedHand = null;

        rb.isKinematic = false;
    }




    private void OnDestroy()
    {
        interactable = false;

        if (connectedHand != null)
        {
            connectedHand.isHoldingObject = false;
        }
    }



    private void OnDrawGizmos()
    {
        if (DEBUGMODE == false)
        {
            return;
        }

        Gizmos.DrawWireSphere(transform.position, objectSize);


        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.TransformPoint(rb.centerOfMass), 0.03f); // Visualize center of mass
        }
    }


    public bool DEBUGMODE;

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy && !Application.isPlaying && Hand.Left != null && Hand.Left.interactionController.settings != null)
        {
            gameObject.layer = Mathf.RoundToInt(Mathf.Log(Hand.Left.interactionController.settings.interactablesLayer.value, 2));
        }
    }
}
