using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBallManager : MonoBehaviour
{
    public static BasketBallManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject basketBallPrefab;
    public Interactable currentBasketBall;



    public Interactable RetrieveBasketBall()
    {
        if (currentBasketBall == null)
        {
            currentBasketBall = Instantiate(basketBallPrefab).GetComponent<Interactable>();

            return currentBasketBall;
        }

        return currentBasketBall;
    }
}
