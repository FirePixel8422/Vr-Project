using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class Tester : MonoBehaviour
{
    public ApplienceSO applience;
    public FoodSO[] foods;

    public List<FoodType> foodTypes;


    private void Start()
    {
        foodTypes = new List<FoodType>();

        AddFoods();
    }

    private void OnValidate()
    {
        if (foodTypes != null)
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
            print("Try Make Food State: " + FoodManager.Instance.TryMakeFood(foodTypes.ToArray(), applience.applience, out Food madeFood) + ", Made: " + madeFood.foodType.foodName);
        }
    }
}
