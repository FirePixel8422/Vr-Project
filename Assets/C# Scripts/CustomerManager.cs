using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;
    private void Awake()
    {
        Instance = this;
    }



    [SerializeField] private Transform[] waypoints;
    
    [HideInInspector] public float3[] wayPointPositions;


    private Customer[] customers;

    [SerializeField] private Food[] foodsForOrder;

    [SerializeField] private float minPatience, maxPatience;



    [BurstCompile]
    private void Start()
    {
        int wayPointCount = waypoints.Length;
        wayPointPositions = new float3[wayPointCount];

        for (int i = 0; i < wayPointCount; i++)
        {
            wayPointPositions[i] = new Vector3(waypoints[i].position.x, 0, waypoints[i].position.z);
        }

        customers = FindObjectsOfType<Customer>();

        for (int i = 0; i < customers.Length; i++)
        {
            customers[i].Init(wayPointPositions);
        }

        SelectNewCustomer();
    }


    [BurstCompile]
    public void SelectNewCustomer()
    {
        int r = Random.Range(0, foodsForOrder.Length);

        FoodType foodToOrder = foodsForOrder[r].foodType.foodType;
        float patience = Random.Range(minPatience, maxPatience);


        r = Random.Range(0, customers.Length);

        customers[r].SetOrder(0, foodToOrder, patience);
    }
}
