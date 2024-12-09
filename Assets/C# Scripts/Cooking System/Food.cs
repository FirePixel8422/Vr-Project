using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;


[BurstCompile]
public class Food : MonoBehaviour
{
    public FoodSO foodType;

    public Recipe requiredRecipe;

    public float minCookTemp = 60;
    public float startBurningTemp = 130, endBurningTemp = 150, turnToDustTemp = 160;

    public bool isCookable;
    public float cookSpeed = 1;
    public float cookPercentage;
    public bool isCooked => cookPercentage >= 100;
    public bool isBurned => cookPercentage >= startBurningTemp + 5;

    private Color baseColor;
    public Color cookedColor;


    private Material material;
    private Rigidbody rb;
    private Collider[] coll;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponentsInChildren<Collider>();

        if (transform.TryGetComponentInChildren(out Renderer renderer, true))
        {
            material = renderer.material;

            baseColor = material.color;
        }
    }

    public void TogglePhysics(bool state)
    {
        rb.isKinematic = !state;

        for (int i = 0; i < coll.Length; i++)
        {
            coll[i].enabled = state;
        }
    }


    [BurstCompile]
    public void Cook(float addedPercentage)
    {
        cookPercentage += addedPercentage * cookSpeed;

        if (cookPercentage <= 100)
        {
            material.color = Color.Lerp(baseColor, cookedColor, cookPercentage / 100);
        }
        else if(cookPercentage >= startBurningTemp && cookPercentage <= endBurningTemp)
        {
            float burnTime = endBurningTemp - startBurningTemp;

            float burnedOverStartBurnTime = cookPercentage - startBurningTemp;

            material.color = Color.Lerp(cookedColor, Color.black, burnedOverStartBurnTime / burnTime);
        }
        else if (cookPercentage >= turnToDustTemp)
        {
            GetComponent<FragmentController>().Shatter(transform.position);
        }
    }
}
