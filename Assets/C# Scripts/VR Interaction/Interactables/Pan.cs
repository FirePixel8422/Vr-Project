using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Pan : Pickupable, ICustomIntervalUpdater_10FPS
{
    [Header("Pan Settings")]

    public float heatUpTime = 45;
    public float coolDownTime = 60;

    public float temparature;
    public float minTemp = 25, maxTemp = 160;

    public bool onGasPit => onGasPitAmount > 0;

    public bool requireUpdate_10FPS => onGasPit || foodList.Count != 0;

    public int onGasPitAmount;

    [Range(0, 1)]
    public float upsideDownThreshold;


    public List<Food> foodList;


    private void Start()
    {
        CustomUpdaterManager.AddUpdater(this);
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ApplienceObject applience) && applience.applience.applienceName == "GasBurner")
        {
            onGasPitAmount += 1;
        }

        if (other.TryGetComponent(out Food food) && food.isCookable)
        {
            food.transform.SetParent(transform, true);
            food.TogglePhysics(false);

            foodList.Add(food);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ApplienceObject applience) && applience.applience.applienceName == "GasBurner")
        {
            onGasPitAmount -= 1;
        }

        if (other.TryGetComponent(out Food food) && foodList.Contains(food))
        {
            food.transform.parent = null;
            food.TogglePhysics(true);

            foodList.Remove(food);
        }
    }


    public void OnIntervalUpdate_10FPS(float deltaTime)
    {
        if (Vector3.Dot(transform.up, Vector3.down) < upsideDownThreshold)
        {
            for (int i = 0; i < foodList.Count; i++)
            {
                if (foodList[i] == null)
                {
                    foodList.RemoveAt(i);
                    i--;
                    continue;
                }

                foodList[i].transform.parent = null;
                foodList[i].TogglePhysics(true);

                foodList.Remove(foodList[i]);
            }
        }

        for (int i = 0; i < foodList.Count; i++)
        {
            if (temparature > foodList[i].minCookTemp)
            {
                foodList[i].Cook(deltaTime * (temparature / maxTemp));
            }
        }

        if (onGasPit)
        {
            temparature = math.clamp(temparature + deltaTime * (maxTemp / heatUpTime), minTemp, maxTemp);
        }
        else
        {
            temparature = math.clamp(temparature - deltaTime * (maxTemp / coolDownTime), minTemp, maxTemp);
        }
    }
}
