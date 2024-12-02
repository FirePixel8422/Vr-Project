using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBallTrigger : MonoBehaviour
{
    public ParticleSystem confetti;

    public VibrationParamaters vibrationParams;

    public bool insideRingLeft;
    public bool insideRingRight;



    private void Start()
    {
        confetti.transform.parent = null;
        transform.parent.GetComponent<Rigidbody>().sleepThreshold = 0.00001f;
    }


    private void OnTriggerExit(Collider coll)
    {
        if (coll.transform.gameObject.CompareTag("Basket"))
        {
            int score = 3;
            bool leftSide = transform.position.z < 0;

            if (leftSide && insideRingLeft)
            {
                score = 2;
            }
            else if (leftSide == false && insideRingRight)
            {
                score = 2;
            }


            coll.GetComponent<BasketScoreCounter>().UpdateScore(score);

            Hand.Left.SendVibration(vibrationParams);
            Hand.Right.SendVibration(vibrationParams);

            confetti.transform.position = transform.position;
            confetti.Play();

            Destroy(transform.parent.gameObject);
            Destroy(confetti.gameObject, 12);
        }
    }
}
