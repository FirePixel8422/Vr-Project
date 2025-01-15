using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


[BurstCompile]
public class Blender : ApplienceObject
{
    [SerializeField] private List<Food> heldFoods;
    [SerializeField] private Transform foodOutputTransform;


    [SerializeField] private float blendTime;
    [SerializeField] private float cooldownTime;

    [SerializeField] private Transform spinner;
    [SerializeField] private float spinnerSpeed;

    [SerializeField] private Vector3 rotAdded;
    [SerializeField] private Vector3 rotClampMin, rotClampMax;

    [SerializeField] private float blendForceDelayMin, blendForceDelayMax;
    [SerializeField] private Vector3 forceOffset;
    [SerializeField] private float minBlendForce, maxBlendForce;
    [SerializeField] private float minBlendUpwardsForce, maxBlendUpwardsForce;




    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == false && other.TryGetComponent(out Food food) && heldFoods.Contains(food) == false)
        {
            heldFoods.Add(food);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger == false && other.TryGetComponent(out Food food))
        {
            heldFoods.Remove(food);
        }
    }



    [BurstCompile]
    private IEnumerator Blending()
    {
        float totalElapsed = 0;
        float elapsed = 0;
        float nextBlendForceTime = Random.Range(blendForceDelayMin, blendForceDelayMax);

        Quaternion startLocalRot = transform.localRotation;

        bool blendingCanceled = false;


        while (totalElapsed < blendTime)
        {
            if (heldFoods.Count == 0)
            {
                blendingCanceled = true;
                break;
            }


            yield return new WaitForFixedUpdate();

            float fixedDeltaTime = Time.fixedDeltaTime;
            float blenderAggresiveness = math.lerp(0, 1.5f, totalElapsed / blendTime);

            //shake the blender violently
            ShakeBlender(fixedDeltaTime, blenderAggresiveness);


            //rotate blades
            spinner.transform.rotation *= Quaternion.Euler(0, spinnerSpeed * fixedDeltaTime * blenderAggresiveness, 0);


            totalElapsed += fixedDeltaTime;
            elapsed += fixedDeltaTime;

            if (elapsed > nextBlendForceTime)
            {
                elapsed = 0;
                nextBlendForceTime = Random.Range(blendForceDelayMin, blendForceDelayMax);

                //move objects physics based, like the blender is moving
                ApplyBlendForce(fixedDeltaTime, blenderAggresiveness);
            }
        }


        totalElapsed = math.clamp(blendTime - totalElapsed, 0, blendTime);

        while (totalElapsed < cooldownTime)
        {
            yield return new WaitForFixedUpdate();

            float fixedDeltaTime = Time.fixedDeltaTime;
            float blenderAggresiveness = math.lerp(1.5f, 0, totalElapsed / cooldownTime);

            //shake the blender violently
            ShakeBlender(fixedDeltaTime, blenderAggresiveness);


            //rotate blades
            spinner.transform.rotation *= Quaternion.Euler(0, spinnerSpeed * fixedDeltaTime * blenderAggresiveness, 0);


            totalElapsed += fixedDeltaTime;
            elapsed += fixedDeltaTime;

            if (elapsed > nextBlendForceTime)
            {
                elapsed = 0;
                nextBlendForceTime = Random.Range(blendForceDelayMin, blendForceDelayMax);

                //move objects physics based, like the blender is moving
                ApplyBlendForce(fixedDeltaTime, blenderAggresiveness);
            }
        }

        //reset to def rotation
        transform.localRotation = startLocalRot;

        if (blendingCanceled)
        {
            StopAllCoroutines();
        }
    }

    [BurstCompile]
    private void ShakeBlender(float fixedDeltaTime, float blenderAggresiveness)
    {
        Vector3 randomRotChange = Random.Range(-rotAdded, rotAdded) * fixedDeltaTime * blenderAggresiveness;

        Vector3 vectorizedRotation = RotationToVector(transform.localEulerAngles);

        Vector3 clampedNewRot = VectorLogic.Clamp(vectorizedRotation + randomRotChange, rotClampMin, rotClampMax);

        transform.localRotation = Quaternion.Euler(clampedNewRot);
    }

    [BurstCompile]
    private Vector3 RotationToVector(Vector3 localEulerAngles)
    {
        for (int i = 0; i < 3; i++)
        {
            if (localEulerAngles[i] > 180)
            {
                localEulerAngles[i] -= 360;
            }
        }

        return localEulerAngles;
    }

    [BurstCompile]
    private void ApplyBlendForce(float fixedDeltaTime, float blenderAggresiveness)
    {
        float _blendForce = Random.Range(minBlendForce, maxBlendForce) * fixedDeltaTime * blenderAggresiveness;
        float _blendUpwardsForce = Random.Range(minBlendUpwardsForce, maxBlendUpwardsForce) * fixedDeltaTime * blenderAggresiveness;

        for (int i = 0; i < heldFoods.Count; ++i)
        {
            heldFoods[i].rb.AddExplosionForce(_blendForce, heldFoods[i].transform.position + Random.Range(-forceOffset, forceOffset), 5, _blendUpwardsForce, ForceMode.VelocityChange);
        }
    }


    [ContextMenu("c")]
    public void StartBlending()
    {
        StartCoroutine(TryBlendRecipe());
    }


    [BurstCompile]
    public IEnumerator TryBlendRecipe()
    {
        yield return StartCoroutine(Blending());

        int heldFoodCount = heldFoods.Count;
        FoodType[] foodTypes = new FoodType[heldFoodCount];

        for (int i = 0; i < heldFoodCount; i++)
        {
            foodTypes[i] = heldFoods[i].foodType.foodType;

            Destroy(heldFoods[i].gameObject);
        }
        heldFoods.Clear();


        if (FoodManager.Instance.TryMakeFood(foodTypes, applience.applience, out Food madeFood))
        {
            Instantiate(madeFood.gameObject, foodOutputTransform.position, Quaternion.identity);
        }
    }
}
