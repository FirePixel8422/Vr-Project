using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;



[BurstCompile]
public class Pan : Pickupable, ICustomIntervalUpdater_10FPS
{
    [Header("Pan Settings")]

    [SerializeField] private float heatUpTime = 45;
    [SerializeField] private float coolDownTime = 60;

    [SerializeField] private float temparature;
    [SerializeField] private float minTemp = 25, maxTemp = 160;



    [SerializeField] private int IsOnGasPitAmount;
    private bool IsOnGasPit => IsOnGasPitAmount > 0;


    [SerializeField] private Transform[] burgerPoints;
    [SerializeField] private List<Food> foodList = new List<Food>();




    private void Start()
    {
        CustomUpdaterManager.AddUpdater(this);
    }


    public override void Pickup(InteractionController hand)
    {
        if (connectedHand != null)
        {
            connectedHand.isHoldingObject = false;
        }

        connectedHand = hand;
        heldByPlayer = true;


        TogglePhysics(false, true);

        bool keepWorldRotation = pickupRotationMode == PickupRotationMode.KeepWorldRotation;
        bool keepPositionOffsetTohand = pickupPositionMode == PickupPositionMode.KeepRelativePosition;

        transform.SetParent(hand.heldItemHolder, keepPositionOffsetTohand, keepWorldRotation);

        connectedHand.SetItemHolderPosition(pickupPosOffset, pickUpRotOffset);


        if (pickupPosOffset != Vector3.zero)
        {
            transform.localPosition += pickupPosOffset;
        }
        if (pickUpRotOffset != Vector3.zero)
        {
            transform.localRotation *= Quaternion.Euler(pickUpRotOffset);
        }

        rb.isKinematic = true;
    }



    #region Detect Food Enter and Exit Pan and Pan Enter and Exit on Stove

    private void OnCollisionEnter(Collision other)
    {
        //check for Stove
        if (other.transform.TryGetComponent(out ApplienceObject applience) && applience.applience.applienceName == "GasBurner")
        {
            IsOnGasPitAmount += 1;
        }

        //get food if not already full
        else if (burgerPoints.Length != foodList.Count && other.transform.TryGetComponent(out Food food) && foodList.Contains(food) == false && food.isCookable)
        {
            food.transform.SetParent(burgerPoints[foodList.Count], false, false);

            food.TogglePhysics(false, true);

            if (food.connectedHand != null)
            {
                food.connectedHand.isHoldingObject = false;
            }

            food.interactable = false;

            foodList.Add(food);
        }
    }



    private void OnCollisionExit(Collision other)
    {
        //check for Stove
        if (other.transform.TryGetComponent(out ApplienceObject applience) && applience.applience.applienceName == "GasBurner")
        {
            IsOnGasPitAmount -= 1;
        }

        //remove food
        else if (other.transform.TryGetComponent(out Food food) && foodList.Contains(food))
        {
            food.transform.parent = null;

            food.TogglePhysics(true);

            food.interactable = true;

            foodList.Remove(food);
        }
    }

    #endregion




    //only update if the Pan is on a gasPit, is not on a gaspit and still needs to cool down or has food in it
    public bool requireUpdate_10FPS => IsOnGasPit || temparature > minTemp || foodList.Count != 0;


    [BurstCompile]
    public void OnIntervalUpdate_10FPS(float deltaTime)
    {
        //all food inside of the pan
        for (int i = 0; i < foodList.Count; i++)
        {
            if (foodList[i] == null || foodList[i].interactable)
            {
                foodList.RemoveAt(i);
                i--;
                continue;
            }


            //cook food 
            if (temparature > foodList[i].minCookTemp)
            {
                foodList[i].Cook(deltaTime * (temparature / maxTemp));
            }
        }


        //heat up / cooldown thepan
        if (IsOnGasPit)
        {
            temparature = math.clamp(temparature + deltaTime * (maxTemp / heatUpTime), minTemp, maxTemp);
        }
        else
        {
            temparature = math.clamp(temparature - deltaTime * (maxTemp / coolDownTime), minTemp, maxTemp);
        }
    }



    private void OnValidate()
    {
        if (pickupPositionMode != PickupPositionMode.SnapToHand)
        {
            pickupPositionMode = PickupPositionMode.SnapToHand;

            Debug.LogWarning("Pan Requires Snap To Hand PickUpPositionMode");
        }

        if (pickupRotationMode != PickupRotationMode.SnapToHand)
        {
            pickupRotationMode = PickupRotationMode.SnapToHand;

            Debug.LogWarning("Pan Requires Snap To Hand PickUpRotationMode");
        }
    }
}
