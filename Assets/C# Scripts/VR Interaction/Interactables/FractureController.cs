using System.Collections;
using UnityEngine;


public class FractureController : MonoBehaviour
{
    public float fractureTreshold;


    public GameObject fracturesParent;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Pickupable pickupable))
        {
            Vector3 vel = collision.relativeVelocity;

            if((vel.x + vel.y + vel.z) * pickupable.weight > fractureTreshold)
            {
                Fracture();
            }
        }
    }

    private void Fracture()
    {
        Destroy(gameObject);

        fracturesParent.transform.parent = null;
        fracturesParent.SetActive(true);
        fracturesParent.transform.DetachChildren();
    }
}