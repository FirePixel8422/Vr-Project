using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : MonoBehaviour
{
    public ParticleSystem confetti;

    public VibrationParamaters vibrationParams;



    private void Start()
    {
        confetti.transform.parent = null;
        transform.parent.GetComponent<Rigidbody>().sleepThreshold = 0.00001f;
    }


    private void OnTriggerExit(Collider coll)
    {
        if (coll.transform.gameObject.CompareTag("Basket"))
        {
            coll.GetComponent<BasketScoreCounter>().UpdateScore();

            Hand.Left.SendVibration(vibrationParams);
            Hand.Right.SendVibration(vibrationParams);

            confetti.transform.position = transform.position;
            confetti.Play();

            Destroy(transform.parent.gameObject);
        }
    }
}
