using System;
using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;


[BurstCompile]
public class InteractionController : MonoBehaviour
{
    private Hand hand;
    private InteractionManager IM;


    [SerializeField]
    private Transform rayTransform;

    [SerializeField]
    private Transform overlapSphereTransform;

    public Transform heldItemHolder;


    private Interactable heldObject;
    public bool isHoldingObject;

    private Interactable toPickupObject;
    public bool objectSelected;


    private Collider[] hitObjectsInSphere;
    private RaycastHit rayHit;



    [BurstCompile]
    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && isHoldingObject == false && objectSelected)
        {
            Pickup();
        }

        if (ctx.canceled && isHoldingObject)
        {
            Drop();
        }
    }


    [BurstCompile]
    private void Start()
    {
        hand = GetComponent<Hand>();

        IM = InteractionManager.Instance;
        hitObjectsInSphere = new Collider[IM.maxExpectedObjectInSphere];

        savedLocalVelocity = new Vector3[frameAmount];
    }


    [BurstCompile]
    private void Update()
    {
        //if you are holding nothing, scan for objects by using a raycast and a sphere around you hand
        if (isHoldingObject == false)
        {
            UpdateToPickObject();
        }

        //if you are holding something and it is throwable (Or "pickupsUseOldHandVel" is true), start doing velocity calculations
        if (IM.pickupsUseOldHandVel || (heldObject != null && heldObject))
        {
            CalculateHandVelocity();
        }
    }




    [BurstCompile]
    private void UpdateToPickObject()
    {
        int interactablesLayer = IM.interactablesLayer;

        //if "GrabState.OnSphereTriger" is true (OverlapSphere is enabled)
        if (IM.grabState.HasFlag(GrabState.OnSphereTrigger))
        {
            Vector3 overlapSphereTransformPos = overlapSphereTransform.position;
            float overlapSphereSize = IM.overlapSphereSize;

            //get all objects near your hand
            int objectsInSphereCount = Physics.OverlapSphereNonAlloc(overlapSphereTransformPos, overlapSphereSize, hitObjectsInSphere, interactablesLayer);


            //resize array if there are too little spots in the Collider Array "hitObjectsInSphere"
            if (objectsInSphereCount > hitObjectsInSphere.Length)
            {
                Debug.LogWarning("Too Little Interaction Slots, Sphere check was resized");

                hitObjectsInSphere = Physics.OverlapSphere(overlapSphereTransformPos, overlapSphereSize, interactablesLayer);
            }


            //if there is atleast 1 object in the sphere
            if (objectsInSphereCount > 0)
            {

                float closestObjectDistance = 10000;
                Interactable new_ToPickupObject = null;
                Interactable targetObject;


                //calculate closest object
                for (int i = 0; i < objectsInSphereCount; i++)
                {
                    targetObject = hitObjectsInSphere[i].GetComponent<Interactable>();

                    float distanceToTargetObject = Vector3.Distance(overlapSphereTransformPos, targetObject.transform.position);

                    if (distanceToTargetObject - targetObject.objectSize < closestObjectDistance)
                    {
                        new_ToPickupObject = targetObject;
                        closestObjectDistance = distanceToTargetObject;
                    }
                }

                //if you are holdijg nothing, or the new_ToPickupObject isnt already selected, select the object and deselect potential previous selected object
                if (objectSelected == false || new_ToPickupObject != toPickupObject)
                {
                    SelectNewObject(new_ToPickupObject);
                }

                return;
            }
        }


        //if "GrabState.OnRaycast" is true (rayCasts are enabled) and there are no objects near your hand, check if there is one in front of your hand
        if (IM.grabState.HasFlag(GrabState.OnRaycast) && Physics.Raycast(rayTransform.position, rayTransform.forward, out rayHit, IM.interactRayCastRange, interactablesLayer, QueryTriggerInteraction.Collide))
        {
            if (rayHit.transform.TryGetComponent(out Interactable new_ToPickupObject))
            {
                //if you are holdijg nothing, or the new_ToPickupObject isnt already selected, select the object and deselect potential previous selected object
                if (objectSelected == false || new_ToPickupObject != toPickupObject)
                {
                    SelectNewObject(new_ToPickupObject);
                }

                return;
            }
        }

        //deselect potential previous selected object
        DeSelectObject();
    }




    #region Select/Deselect Object

    [BurstCompile]
    private void SelectNewObject(Interactable new_ToPickupObject)
    {
        if (objectSelected)
        {
            toPickupObject.DeSelect();
        }

        toPickupObject = new_ToPickupObject;

        objectSelected = true;

        hand.SendVibration(IM.selectPickupVibrationParams);
    }

    [BurstCompile]
    private void DeSelectObject()
    {
        if (objectSelected)
        {
            toPickupObject.DeSelect();
        }

        objectSelected = false;
    }

    #endregion




    #region Drop and Pickup

    [BurstCompile]
    public void Pickup()
    {
        //if the object that is trying to be picked up by this hand, is held by the other hand and canSwapItemFromHands is false, return
        if (toPickupObject.heldByPlayer && IM.canSwapItemFromHands == false)
        {
            return;
        }

        toPickupObject.Pickup(this);

        heldObject = toPickupObject;
        isHoldingObject = true;

        hand.SendVibration(IM.pickupVibrationParams);
    }


    [BurstCompile]
    private void Drop()
    {
        //drop item if it is throwable
        if (heldObject.isThrowable)
        {
            Vector3 velocity = Vector3.zero;

            for (int i = 0; i < frameAmount; i++)
            {
                velocity += savedLocalVelocity[i] / frameAmount;
            }

            heldObject.Throw(velocity * IM.throwVelocityMultiplier);
        }
        else
        {
            heldObject.Drop();
        }

        heldObject = null;
        isHoldingObject = false;
    }

    #endregion




    #region CalculateHandVelocity


    private Vector3 prevTransformPos;
    private Vector3[] savedLocalVelocity;

    public int frameAmount;
    private int frameIndex;


    [BurstCompile]
    private void CalculateHandVelocity()
    {
        //Calculate hand velocity based on hand movement

        savedLocalVelocity[frameIndex] = (transform.localPosition - prevTransformPos) / Time.deltaTime;

        frameIndex += 1;
        if (frameIndex == frameAmount)
        {
            frameIndex = 0;
        }

        prevTransformPos = transform.localPosition;
    }

    #endregion




    #region Spawn BasketBall in Hand

    public void TEMP_SpawnBall(InputAction.CallbackContext ctx)
    {
        if (ctx.performed == false || heldObject != null)
        {
            return;
        }

        Interactable toPickupObject = BasketBallManager.Instance.RetrieveBasketBall();

        toPickupObject.Pickup(this);

        heldObject = toPickupObject;
        isHoldingObject = true;

        hand.SendVibration(IM.pickupVibrationParams);
    }

    #endregion

}