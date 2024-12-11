using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerSpatula : MonoBehaviour, ICustomLateUpdater
{
    public Transform foodHolderTransform;

    public Food heldFood;



    private void Start()
    {
        CustomUpdaterManager.AddUpdater(this);
    }



    private void OnTriggerEnter(Collider coll)
    {
        if (heldFood == null && coll.TryGetComponent(out Food food) && food.IsCooked)
        {
            heldFood = food;
            heldFood.transform.SetParent(foodHolderTransform, false, false);
            heldFood.interactable = true;

            food.TogglePhysics(true);
            food.TogglePhysics(false, true);
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (heldFood != null && coll.TryGetComponent(out Food food) && food.IsCooked)
        {
            heldFood = null;

            food.TogglePhysics(true);
        }
    }


    public bool requireLateUpdate => heldFood != null;

    public void OnLateUpdate()
    {
        heldFood.transform.position = foodHolderTransform.position;
    }
}
