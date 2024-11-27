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
    }


    private void OnTriggerExit(Collider coll)
    {
        if (coll.transform.gameObject.CompareTag("Basket"))
        {
            Hand.Left.SendVibration(vibrationParams);
            Hand.Right.SendVibration(vibrationParams);

            confetti.transform.position = transform.position;
            confetti.Play();

            Destroy(transform.parent.gameObject);
        }
    }
}
