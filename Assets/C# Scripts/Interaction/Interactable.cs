using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
[BurstCompile]
public class Interactable : MonoBehaviour
{
    public bool interactable = true;
    public bool isThrowable = true;
    public bool heldByPlayer;

    private InteractionController connectedHand;

    public float throwVelocityMultiplier = 1;
    public Vector3 velocityClamp = new Vector3(3, 3, 3);


    public float objectSize;


    private Rigidbody rb;


    [BurstCompile]
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }



    [BurstCompile]
    public void Select()
    {

    }

    [BurstCompile]
    public void DeSelect()
    {

    }


    [BurstCompile]
    public void Pickup(InteractionController hand)
    {
        if (connectedHand != null)
        {
            connectedHand.isHoldingObject = false;
        }

        connectedHand = hand;
        heldByPlayer = true;

        transform.SetParent(hand.heldItemHolder, false, false);
        rb.isKinematic = true;
    }


    [BurstCompile]
    public void Throw(Vector3 velocity)
    {
        connectedHand = null;
        heldByPlayer = false;

        transform.parent = null;
        connectedHand = null;

        rb.isKinematic = false;
        rb.velocity = VectorLogic.ClampDirection(velocity * throwVelocityMultiplier, velocityClamp);
    }


    [BurstCompile]
    public void Drop()
    {
        connectedHand = null;
        heldByPlayer = false;

        transform.parent = null;
        connectedHand = null;

        rb.isKinematic = false;
    }



    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, objectSize);   
    }

    private void OnValidate()
    {
        if (gameObject.activeInHierarchy && !Application.isPlaying)
        {
            InteractionManager IM = FindObjectOfType<InteractionManager>();

            if (IM == null)
            {
                Debug.LogError("There is no InteractionManager, Please add it now.");
                return;
            }
            gameObject.layer = Mathf.RoundToInt(Mathf.Log(IM.interactablesLayer.value, 2));
        }
    }
}
