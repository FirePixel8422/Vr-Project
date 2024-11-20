using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Recipe", menuName = "Cooking System/Recipe", order = 0)]
public class RecipeSO : ScriptableObject
{
    public Recipe recipe;
}



[System.Serializable]
public struct Recipe
{
    public FoodSO[] requiredFood;

    public ApplienceSO requiredApplience;
}