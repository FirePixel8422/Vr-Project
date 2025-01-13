using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : ApplienceObject, ICustomUpdater
{
    private bool ContainsFood => foodList.Count > 0;

    [SerializeField] private List<Food> foodList = new List<Food>();
    [SerializeField] private List<float> foodCookedPercentList = new List<float>();

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
            foodCookedPercentList.Add(0);
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
                    foodCookedPercentList.RemoveAt(i);
                }
            }
        }
    }


    public bool requireUpdate => ContainsFood;

    public void OnUpdate()
    {
        for (int i = 0; i < foodCookedPercentList.Count; i++)
        {
            foodCookedPercentList[i] += cookSpeed * Time.deltaTime;

            if (foodCookedPercentList[i] >= 100)
            {
                if (FoodManager.Instance.TryMakeFood(new FoodType[1] { foodList[i].foodType.foodType }, applience.applience, out Food madeFood))
                {
                    Instantiate(madeFood.gameObject, foodOutputPoint.position, Quaternion.identity);
                }

                Destroy(foodList[i].gameObject);

                foodList.RemoveAt(i);
                foodCookedPercentList.RemoveAt(i);
            }
        }
    }
}
