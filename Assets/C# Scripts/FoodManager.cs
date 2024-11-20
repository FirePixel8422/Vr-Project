using Unity.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using Unity.Jobs;
using Unity.VisualScripting;


public class FoodManager : MonoBehaviour
{
    public RecipeSO[] recipesSOList;

    public static Recipe[] recipesList;



    private void Start()
    {
        recipesList = new Recipe[recipesSOList.Length];

        for (int i = 0; i < recipesList.Length; i++)
        {
            recipesList[i] = recipesSOList[i].recipe;
        }
    }



    public static bool TryMakeFood(Applience targetApplience, NativeList<FoodType> foods, NativeList<int> requiredFoods)
    {
        int foodAmount = foods.Length;
        for (int i = 0; i < requiredFoods.Length; i++)
        {
            foodAmount += requiredFoods[i] - 1;
        }


        for (int i = 0; i < recipesList.Length; i++)
        {

            //if target recipe has a different Applience requirement OR has more/less foods then current amount of foods, skip recipe check
            if (recipesList[i].requiredApplience.applience != targetApplience || recipesList[i].requiredFood.Length != foodAmount)
            {
                continue;
            }




            int[] amountOfFoods = new int[foodAmount];
            

            //loop over all currentRecipes foods
            for (int recipeFoodIndex = 0; recipeFoodIndex < recipesList[i].requiredFood.Length; recipeFoodIndex++)
            {

                //Loop over all currentFoods
                for (int targetFoodIndex = recipeFoodIndex; targetFoodIndex < foods.Length; targetFoodIndex++)
                {

                    //check if currentFood is in currentRecipes foods
                    if (foods[targetFoodIndex] == recipesList[i].requiredFood[recipeFoodIndex].foodType)
                    {

                        //if food is NOT already added
                        if (amountOfFoods[targetFoodIndex] != requiredFoods[targetFoodIndex])
                        {
                            amountOfFoods[targetFoodIndex] += 1;

                            break;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }



            //check if all food matches the recipe
            int foodMatches = 0;

            for (int i2 = 0; i2 < foods.Length; i2++)
            {
                if (amountOfFoods[i2] == requiredFoods[i2])
                {
                    foodMatches += 1;
                }
            }

            if (foodMatches == foodAmount)
            {
                return true;
            }
        }


        return false;
    }
}
