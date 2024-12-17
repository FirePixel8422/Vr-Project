using System.Collections;
using UnityEngine;

public class InfiniBurger : Interactable
{
    public Food burgerPrefab;


    public override void Pickup(InteractionController hand)
    {
        Food burger = Instantiate(burgerPrefab, hand.transform.position, hand.transform.rotation);

        StartCoroutine(Delay(hand, burger));
    }


    private IEnumerator Delay(InteractionController hand, Interactable burger)
    {
        yield return new WaitForEndOfFrame();

        hand.Pickup(burger);
    }
}
