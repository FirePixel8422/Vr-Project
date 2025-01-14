using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoilingPan : ApplienceObject, ICustomUpdater
{
    private bool ContainsFood => foodList.Count > 0;

    [SerializeField] private List<Food> foodList = new List<Food>();
    [SerializeField] private float foodCookedPercent;

    [SerializeField] private float cookSpeed;

    [SerializeField] private Transform foodOutputPoint;


    private void Start()
    {
        CustomUpdaterManager.AddUpdater(this);
    }


    private void OnTriggerEnter(Collider other)
    {
        //check for Food
        if (other.isTrigger == false && other.transform.TryGetComponent(out Food food) && foodList.Contains(food) == false)
        {
            foodList.Add(food);
            foodCookedPercent = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //check for Food
        if (other.isTrigger == false && other.transform.TryGetComponent(out Food food))
        {
            for (int i = 0; i < foodList.Count; i++)
            {
                if (foodList[i] == food)
                {
                    foodList.RemoveAt(i);
                    foodCookedPercent = 0;
                }
            }
        }
    }


    public bool requireUpdate => ContainsFood;

    public void OnUpdate()
    {
        foodCookedPercent += cookSpeed * Time.deltaTime;

        if (foodCookedPercent >= 100)
        {
            int foodCount = foodList.Count;
            FoodType[] foods = new FoodType[foodCount];

            for (int i = 0; i < foodCount; i++)
            {
                foods[i] = foodList[i].foodType.foodType;

                Destroy(foodList[i].gameObject);
            }


            if (FoodManager.Instance.TryMakeFood(foods, applience.applience, out Food madeFood))
            {
                Instantiate(madeFood.gameObject, foodOutputPoint.position, Quaternion.identity);
            }

            foodCookedPercent = 0;
        }
    }
}
