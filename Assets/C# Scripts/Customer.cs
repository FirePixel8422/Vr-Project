using System.Collections;
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

    [SerializeField] private float minAFKTime, maxAFKTime;

    [SerializeField] private float patience;

    [SerializeField] private Vector3 centerRoamPoint;
    [SerializeField] private Vector3 nextRoamPoint;
    [SerializeField] private float maxRoamDist;

    [SerializeField] private FoodType requestedFoodType;


    private Animator anim;
    [SerializeField] private bool updateAIPath = true;
    [SerializeField] private bool ordering = false;
    [SerializeField] private bool roaming = true;




    [BurstCompile]
    public void Init(float3[] wayPointPositions)
    {
        CustomUpdaterManager.AddUpdater(this);

        orderWayPoints = wayPointPositions;

        nextRoamPoint = new Vector3(centerRoamPoint.x + Random.Range(-maxRoamDist, maxRoamDist), 0, centerRoamPoint.z + Random.Range(-maxRoamDist, maxRoamDist));

        anim = GetComponent<Animator>();
    }


    [BurstCompile]
    public void SetOrder(int _wayPointIndex, FoodType _requestedFoodType, float maxPatience)
    {
        wayPointIndex = _wayPointIndex;
        requestedFoodType = _requestedFoodType;
        
        patience = maxPatience;


        StopAllCoroutines();
        roaming = false;

        ordering = true;
    }


    public bool requireUpdate => updateAIPath;

    [BurstCompile]
    public void OnUpdate()
    {
        if (ordering)
        {
            bool destinationReached = MoveToNextOrderWaypoint(orderWayPoints[wayPointIndex]);

            if (destinationReached)
            {
                if (wayPointIndex == 0)
                {
                    anim.SetBool("Walking", false);

                    ordering = false;

                    StartCoroutine(WaitForFoodTimer(patience));
                }

                wayPointIndex += 1;

                if (wayPointIndex == orderWayPoints.Length)
                {
                    wayPointIndex = 0;
                }
            }
        }
        else if (roaming)
        {
            bool destinationReached = MoveToNextOrderWaypoint(nextRoamPoint);

            if (destinationReached)
            {
                anim.SetBool("Walking", false);

                roaming = false;

                StartCoroutine(AFKTimer(Random.Range(minAFKTime, maxAFKTime)));
            }
            
        }
    }


    [BurstCompile]
    private bool MoveToNextOrderWaypoint(Vector3 destination)
    {
        Vector3 playerXZPos = new Vector3(transform.position.x, 0, transform.position.z);

        //new position towards next waypoint
        Vector3 newPlayerXYPos = VectorLogic.InstantMoveTowards(playerXZPos, destination, moveSpeed * Time.deltaTime);


        //rotate customer
        Vector3 dir = Vector3.Normalize(newPlayerXYPos - playerXZPos);

        Quaternion targetRot = Quaternion.Euler(0, math.atan2(dir.x, dir.z) * Mathf.Rad2Deg, 0);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

        //if looking fully towards walk direction
        if (Quaternion.Angle(transform.rotation, targetRot) > minRotAccuracyBeforeMoving)
        {
            anim.SetBool("Walking", false);

            return false;
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
        if (Vector3.Distance(newPlayerXYPos, destination) < 0.05f)
        {
            return true;
        }


        return false;
    }


    [BurstCompile]
    private IEnumerator WaitForFoodTimer(float patience)
    {
        yield return new WaitForSeconds(patience);

        roaming = true;

        CustomerManager.Instance.SelectNewCustomer();
    }


    [BurstCompile]
    private IEnumerator AFKTimer(float AFKTime)
    {
        yield return new WaitForSeconds(AFKTime);

        nextRoamPoint = new Vector3(centerRoamPoint.x + Random.Range(-maxRoamDist, maxRoamDist), 0, centerRoamPoint.z + Random.Range(-maxRoamDist, maxRoamDist));

        roaming = true;
    }



    [BurstCompile]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Food food) && food.foodType.foodType == requestedFoodType)
        {
            Destroy(food.gameObject);

            print("mission Complete");
        }
    }



#if UNITY_EDITOR

    [SerializeField] private bool drawGizmos;

    private void OnDrawGizmos()
    {
        if (drawGizmos == false)
        {
            return;
        }

        Gizmos.DrawWireSphere(centerRoamPoint, maxRoamDist);
      
        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(centerRoamPoint, .5f);
    }
#endif
}
