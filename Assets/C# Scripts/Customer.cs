using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;


[BurstCompile]
public class Customer : MonoBehaviour, ICustomUpdater
{
    [SerializeField] private float3[] orderWayPoints;
    [SerializeField] private int wayPointIndex;

    [SerializeField] private float moveSpeed;

    [SerializeField] private float rotSpeed;
    [SerializeField] private float minRotAccuracyBeforeMoving;

    [SerializeField] private float patience;

    [SerializeField] private FoodType requestedFoodType;


    private Animator anim;
    private bool updateAIpath = true;
    private bool ordering = true;




    [BurstCompile]
    private void Start()
    {
        CustomUpdaterManager.AddUpdater(this);

        anim = GetComponent<Animator>();
    }


    [BurstCompile]
    public void SetOrder(int _wayPointIndex, FoodType _requestedFoodType, float maxPatience)
    {
        wayPointIndex = _wayPointIndex;
        requestedFoodType = _requestedFoodType;
        
        patience = maxPatience;

        ordering = true;
    }


    public bool requireUpdate => updateAIpath;

    [BurstCompile]
    public void OnUpdate()
    {
        if (ordering)
        {
            MoveToNextOrderWaypoint();
        }
    }


    [BurstCompile]
    private void MoveToNextOrderWaypoint()
    {
        Vector3 playerXZPos = new Vector3(transform.position.x, 0, transform.position.z);

        //new position towards next waypoint
        Vector3 newPlayerXYPos = VectorLogic.InstantMoveTowards(playerXZPos, orderWayPoints[wayPointIndex], moveSpeed * Time.deltaTime);


        //rotate customer
        Vector3 dir = Vector3.Normalize(newPlayerXYPos - playerXZPos);

        Quaternion targetRot = Quaternion.Euler(0, math.atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        //if looking fully towards walk direction
        if (Quaternion.Angle(transform.rotation, targetRot) > minRotAccuracyBeforeMoving)
        {
            anim.SetBool("Walking", false);

            return;
        }
        else
        {
            anim.SetBool("Walking", true);
        }


        //keep feet on ground with ray
        if (Physics.Raycast(transform.position + Vector3.up * 3, Vector3.down, out RaycastHit hit))
        {
            transform.position = new Vector3(newPlayerXYPos.x, hit.point.y, newPlayerXYPos.z);
        }


        //if waypoint has been reached
        if (Vector3.Distance(newPlayerXYPos, orderWayPoints[wayPointIndex]) < 0.05f)
        {
            if (wayPointIndex == 0)
            {
                anim.SetBool("Walking", false);
                updateAIpath = false;

                StartCoroutine(WaitForFoodTimer(patience));
            }

            wayPointIndex += 1;

            if (wayPointIndex == orderWayPoints.Length)
            {
                wayPointIndex = 0;
            }
        }
    }


    [BurstCompile]
    private IEnumerator WaitForFoodTimer(float patience)
    {
        yield return new WaitForSeconds(patience);

        updateAIpath = true;

        CustomerManager.Instance.SelectNewCustomer();
    }



    [BurstCompile]
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Food food) && food.foodType.foodType == requestedFoodType)
        {

        }
    }
}
