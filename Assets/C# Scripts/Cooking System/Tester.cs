using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public ApplienceSO applience;
    public FoodSO[] foods;

    public NativeList<FoodType> foodTypes;


    private void Start()
    {
        foodTypes = new NativeList<FoodType>(100, Allocator.Persistent);

        AddFoods();
    }

    private void OnValidate()
    {
        if (foodTypes.IsCreated)
        {
            foodTypes.Clear();

            AddFoods();
        }
    }

    private void AddFoods()
    {
        for (int i = 0; i < foods.Length; i++)
        {
            foodTypes.Add(foods[i].foodType);
        }
    }


    public bool trigger;

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            print("Try Make Food State: " + FoodManager.Instance.TryMakeFood(foodTypes, applience.applience, out Food madeFood) + ", Made: " + madeFood.foodType.foodName);
        }
    }
}
