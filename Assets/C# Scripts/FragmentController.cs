using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;


[BurstCompile]
public class FragmentController : MonoBehaviour
{
    [SerializeField] private float fractureThreshold;

    [SerializeField] private FragmentScalar shatterObj;

    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float upwardsModifier;

    [SerializeField] private float decayDelay;
    [SerializeField] private float decaySpeed;



    [BurstCompile]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.HasComponent<Pickupable>())
        {
            float3 vel = collision.rigidbody.velocity;

            float calcVel = vel.x + vel.y + vel.z;

            if (calcVel > fractureThreshold)
            {
                Shatter(collision.GetContact(0).point);
            }
        }
    }

    [BurstCompile]
    public void Shatter(Vector3 shatterCenterPoint)
    {
        Rigidbody[] shatterPieces = shatterObj.GetComponentsInChildren<Rigidbody>();

        int shatterPieceCount = shatterPieces.Length;
        for (int i = 0; i < shatterPieceCount; i++)
        {
            shatterPieces[i].AddExplosionForce(Random.Range(0, explosionForce), shatterCenterPoint, explosionRadius, upwardsModifier, ForceMode.VelocityChange);
        }

        shatterObj.transform.parent = null;
        shatterObj.gameObject.SetActive(true);
        shatterObj.StartCoroutine(shatterObj.SchrinkFragments(shatterPieces, decayDelay, decaySpeed));


        Destroy(gameObject);
    }
}
