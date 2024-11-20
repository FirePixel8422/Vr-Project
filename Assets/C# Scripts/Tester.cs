using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public ApplienceSO applience;
    public FoodSO[] foods;

    public NativeList<FoodType> foodTypes;
    public NativeList<int> foodAmounts;


    private void Start()
    {
        Init();
    }

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        foodTypes = new NativeList<FoodType>(100, Allocator.Persistent);
        foodAmounts = new NativeList<int>(100, Allocator.Persistent);

        for (int i = 0; i < foods.Length; i++)
        {
            int stackedFoodIndex = -1;

            for (int i2 = 0; i2 < foodTypes.Length; i2++)
            {
                if (foodTypes[i2] == foods[i].foodType)
                {
                    stackedFoodIndex = i2;
                    break;
                }
            }

            if (stackedFoodIndex != -1)
            {
                foodAmounts[stackedFoodIndex] += 1;
            }
            else
            {
                foodTypes.Add(foods[i].foodType);
                foodAmounts.Add(1);
            }
        }
    }


    public bool trigger;

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            print("Try Make Food State: " + FoodManager.TryMakeFood(applience.applience, foodTypes, foodAmounts));
        }
    }
}
