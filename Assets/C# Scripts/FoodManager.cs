using Unity.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using Unity.Jobs;
using Unity.VisualScripting;


public class FoodManager : MonoBehaviour
{
    public static FoodManager Instance;
    private void Awake()
    {
        Instance = this;
    }




    public Food[] recipesList;




    public bool TryMakeFood(NativeList<FoodType> foods, Applience targetApplience, out Food madeFood)
    {
        madeFood = null;

        print(foods.Length);

        Recipe attemptedRecipe = new Recipe(foods.AsArray().ToArray(), targetApplience);


        for (int i = 0; i < recipesList.Length; i++)
        {
            if (recipesList[i].requiredRecipe == attemptedRecipe)
            {
                madeFood = recipesList[i];
                return true;
            }
        }


        return false;
    }
}
