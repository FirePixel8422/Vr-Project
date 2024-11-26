using System.Collections;
using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;


public class InteractionController : MonoBehaviour
{
    public LayerMask interactablesLayer;
    public float interactRange;
    public bool canSwapItemFromHands;

    public Transform handTransform;
    public Transform heldItemHolder;

    private Interactable heldObject;


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

            heldObject.Throw(velocity);
        }

        heldObject = null;
    }



    public GameObject ball;

    public void TEMP_SpawnBall(InputAction.CallbackContext ctx)
    {
        if(ctx.performed == false || heldObject != null)
        {
            return;
        }

        GameObject ballObj = Instantiate(ball);

        Interactable hitItem = ballObj.GetComponent<Interactable>();

        hitItem.transform.SetParent(heldItemHolder, false, false);
        hitItem.heldByPlayer = true;

        heldObject = hitItem;
        heldObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}