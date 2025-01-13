using System.Collections;
using UnityEngine;

public class InfiniFood : Interactable
{
    public Food foodPrefab;


    public override void Pickup(InteractionController hand)
    {
        Food foodObj = Instantiate(foodPrefab, hand.transform.position, hand.transform.rotation);

        StartCoroutine(Delay(hand, foodObj));
    }


    private IEnumerator Delay(InteractionController hand, Interactable foodObj)
    {
        yield return new WaitForEndOfFrame();

        hand.Pickup(foodObj);
    }
}
