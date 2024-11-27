using System.Collections;
using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;


public class InteractionController : MonoBehaviour
{
    public LayerMask interactablesLayer;
    public float interactRange;
    public bool canSwapItemFromHands;

    public float throwVlocityMultiplier;

    public Transform handTransform;
    public Transform heldItemHolder;

    private Interactable heldObject;

    
    public HapticImpulsePlayer hapticImpulsePlayer;

    public float amplitude;
    public float frequency;
    public float duration;



    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && heldObject == null)
        {
            TryPickup();
        }

        if (ctx.canceled && heldObject != null)
        {
            Drop();
        }
    }


    public void TryPickup()
    {
        if (Physics.Raycast(handTransform.position, handTransform.forward, out RaycastHit hit, interactRange, interactablesLayer, QueryTriggerInteraction.Collide))
        {
            if (hit.transform.TryGetComponent(out Interactable hitItem))
            {
                if (hitItem.heldByPlayer)
                {
                    if (canSwapItemFromHands == false)
                    {
                        return;
                    }

                    hitItem.connectedHand.heldObject = null;
                }

                hitItem.Pickup(this);

                SendVibration(amplitude, duration, frequency);

                heldObject = hitItem;
            }
        }
    }




    private Vector3 prevPos;
    private Vector3[] savedVelocity;

    public int frameAmount;
    private int savedVelocityIndex;


    private void Start()
    {
        savedVelocity = new Vector3[frameAmount];
    }

    private void Update()
    {
        if (heldObject == null || heldObject.isThrowable == false)
        {
            return;
        }

        savedVelocity[savedVelocityIndex] = (handTransform.position - prevPos) / Time.deltaTime;

        savedVelocityIndex += 1;
        if(savedVelocityIndex == frameAmount)
        {
            savedVelocityIndex = 0;
        }

        prevPos = handTransform.position;
    }

    [BurstCompile]
    private void Drop()
    {
        if (heldObject.isThrowable)
        {
            Vector3 velocity = Vector3.zero;

            for (int i = 0; i < frameAmount; i++)
            {
                velocity += savedVelocity[i] / frameAmount;
            }

            heldObject.Throw(velocity * throwVlocityMultiplier);
        }

        heldObject = null;
    }



    public void TEMP_SpawnBall(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == false || heldObject != null)
        {
            return;
        }

        Interactable hitItem = BasketBallManager.Instance.RetrieveBasketBall();

        hitItem.rb = hitItem.transform.GetComponent<Rigidbody>();
        hitItem.Pickup(this);

        SendVibration(amplitude, duration, frequency);

        heldObject = hitItem;
    }


    public void SendVibration(float amplitude, float duration, float frequency)
    {
        hapticImpulsePlayer.SendHapticImpulse(amplitude, duration, frequency);
    }



    private void OnDrawGizmos()
    {
        Debug.DrawLine(handTransform.position, handTransform.position + handTransform.forward * 1000, Color.red);
    }
}