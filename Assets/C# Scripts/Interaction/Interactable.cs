using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    public bool interactable = true;
    public bool isThrowable = true;
    public bool heldByPlayer;

    public InteractionController connectedHand;

    public float throwVelocityMultiplier = 1;
    public Vector3 velocityClamp = new Vector3(3, 3, 3);


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
        connectedHand = null;
        heldByPlayer = false;

        transform.parent = null;
        connectedHand = null;

        rb.isKinematic = false;
        rb.velocity = VectorLogic.ClampDirection(velocity * throwVelocityMultiplier, velocityClamp);
    }
}
