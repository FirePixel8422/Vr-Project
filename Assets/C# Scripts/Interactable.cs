using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    public bool interactable;
    public bool isThrowable;
    public bool heldByPlayer;

    public InteractionController connectedHand;

    public float throwVelocityMultiplier;
    public Vector3 velocityClamp;


    [HideInInspector]
    public Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }



    public void Pickup(InteractionController hand)
    {
        connectedHand = hand;
        heldByPlayer = true;

        transform.SetParent(hand.heldItemHolder, false, false);
        rb.isKinematic = true;
    }


    public void Throw(Vector3 velocity)
    {
        transform.parent = null;
        connectedHand = null;

        rb.isKinematic = false;
        rb.velocity = VectorLogic.ClampDirection(velocity * throwVelocityMultiplier, velocityClamp);

        heldByPlayer = false;
    }
}
