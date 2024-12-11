using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;


[BurstCompile]
public class Food : Pickupable
{
    public FoodSO foodType;

    public Recipe requiredRecipe;

    public float minCookTemp = 60;
    public float startBurningPercent = 130, endBurningPercent = 150, turnToDustPercent = 160;

    public bool isCookable;
    public float cookSpeed = 1;
    public float cookPercentage;
    public bool IsCooked => isCookable && cookPercentage >= 100;
    public bool IsBurned => cookPercentage >= startBurningPercent;

    private Color baseColor;
    public Color cookedColor;


    private Material material;


    private void Start()
    {
        if (transform.TryGetComponentInChildren(out Renderer renderer, true))
        {
            material = renderer.material;

            baseColor = material.color;
        }
    }



    [BurstCompile]
    public void Cook(float addedPercentage)
    {
        cookPercentage += addedPercentage * cookSpeed;

        //cook
        if (cookPercentage <= 100)
        {
            material.color = Color.Lerp(baseColor, cookedColor, cookPercentage / 100);
        }
        //burn
        else if(cookPercentage >= startBurningPercent && cookPercentage <= endBurningPercent)
        {
            float burnTime = endBurningPercent - startBurningPercent;

            float burnedOverStartBurnTime = cookPercentage - startBurningPercent;

            material.color = Color.Lerp(cookedColor, Color.black, burnedOverStartBurnTime / burnTime);
        }
        //turn to dust
        else if (cookPercentage >= turnToDustPercent)
        {
            cookPercentage = -100;
            GetComponent<FragmentController>().Shatter(transform.position);
        }
    }
}
