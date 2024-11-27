using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : MonoBehaviour
{
    public ParticleSystem confetti;

    public float amplitude;
    public float frequency;
    public float duration;



    private void Start()
    {
        confetti.transform.parent = null;
    }


    private void OnTriggerExit(Collider coll)
    {
        if (coll.transform.gameObject.CompareTag("Basket"))
        {
            InteractionController[] hapticImpulsePlayer = FindObjectsOfType<InteractionController>(true);
            for (int i = 0; i < hapticImpulsePlayer.Length; i++)
            {
                hapticImpulsePlayer[i].SendVibration(amplitude, duration, frequency);
            }

            confetti.transform.position = transform.position;
            confetti.Play();

            Destroy(transform.parent.gameObject);
        }
    }
}
