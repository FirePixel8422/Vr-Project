using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;


[BurstCompile]
public class Food : Pickupable
{
    public FoodSO foodType;

    public Recipe requiredRecipe;
}