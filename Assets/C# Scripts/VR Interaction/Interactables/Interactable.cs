using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;




[BurstCompile]
public class Interactable : MonoBehaviour
{
    protected InteractionController connectedHand;

    public bool interactable = true;
    public bool isThrowable = true;
    public bool heldByPlayer;

    public float objectSize;



    #region Select And Deselect

    [BurstCompile]
    public virtual void OnSelect()
    {

    }

    [BurstCompile]
    public virtual void OnDeSelect()
    {

    }

    #endregion




    #region Pickup, Throw And Drop

    [BurstCompile]
    public void Pickup(InteractionController hand)
    {
        if (connectedHand != null)
        {
            connectedHand.isHoldingObject = false;
        }

        connectedHand = hand;
        heldByPlayer = true;

        OnPickup(hand);
    }
    protected virtual void OnPickup(InteractionController hand)
    {
        return;
    }




    [BurstCompile]
    public void Throw(Vector3 velocity, Vector3 angularVelocity)
    {
        connectedHand = null;
        heldByPlayer = false;

        OnThrow(velocity, angularVelocity);
    }
    protected virtual void OnThrow(Vector3 velocity, Vector3 angularVelocity)
    {
        return;
    }




    [BurstCompile]
    public void Drop()
    {
        connectedHand = null;
        heldByPlayer = false;

        OnDrop();
    }
    protected virtual void OnDrop()
    {
        return;
    }

    #endregion




    private void OnDestroy()
    {
        interactable = false;

        if (connectedHand != null)
        {
            connectedHand.isHoldingObject = false;
        }
    }


    private void OnValidate()
    {
        if (gameObject.activeInHierarchy && !Application.isPlaying && Hand.Left != null && Hand.Left.interactionController.settings != null)
        {
            gameObject.layer = Mathf.RoundToInt(Mathf.Log(Hand.Left.interactionController.settings.interactablesLayer.value, 2));
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, objectSize);
    }
}
