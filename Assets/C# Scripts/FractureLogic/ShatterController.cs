using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;


[BurstCompile]
public class ShatterController : FragmentController
{
    [SerializeField] private float health;
    [SerializeField] private float fractureThreshold;




#if UNITY_EDITOR

    [Header("\n[Debug]")]
    [SerializeField] private Transform shatterFromPoint;

    [ContextMenu("Shatter This Object From ShatterFromPoint Transform")]
    private void ShatterEditorFromTransform()
    {
        try
        {
            if (Application.isPlaying)
            {
                Vector3 pos = GetComponent<Collider>().ClosestPoint(shatterFromPoint.position) - (transform.position - shatterFromPoint.position).normalized;
                Shatter(pos);
            }
        }
        catch (System.Exception e)
        {
            throw e;
        }
    }
#endif


    [BurstCompile]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out Pickupable pickupable))
        {
            float3 vel = collision.rigidbody.velocity;

            float calcVel = (math.abs(vel.x) + math.abs(vel.y) + math.abs(vel.z)) * pickupable.weight;

            if (calcVel > fractureThreshold)
            {
                health -= calcVel;

                if (health <= 0)
                {
                    Vector3 pos = GetComponent<Collider>().ClosestPoint(pickupable.transform.position - (transform.position - pickupable.transform.position).normalized);
                    Shatter(pos);
                }
            }
        }
    }
}
