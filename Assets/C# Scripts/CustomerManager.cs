using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

[BurstCompile]
public class CustomerManager : MonoBehaviour, ICustomUpdater
{
    public static CustomerManager Instance;
    private void Awake()
    {
        Instance = this;
    }


    public int serverCount;
    public float timeLeft;
    public TextMeshProUGUI scoreTextObj;
    public TextMeshProUGUI timeTextObj;
    public TextMeshProUGUI timesUpTextObj;


    [SerializeField] private Transform[] waypoints;
    
    [HideInInspector] public float3[] wayPointPositions;


    private Customer[] customers;

    [SerializeField] private float minPatience, maxPatience;


    [BurstCompile]
    private void Start()
    {
        CustomUpdaterManager.AddUpdater(this);

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

        SelectNewCustomer(false);
    }


    [BurstCompile]
    public void SelectNewCustomer(bool succes)
    {
        int r = Random.Range(0, FoodManager.Instance.recipesList.Length);

        FoodType foodToOrder = FoodManager.Instance.recipesList[r].foodType.foodType;
        Sprite sprite = FoodManager.Instance.recipeSprites[r];

        float patience = Random.Range(minPatience, maxPatience);


        r = Random.Range(0, customers.Length);

        customers[r].SetOrder(0, foodToOrder, patience, sprite);

        if (succes)
        {
            serverCount += 1;

            scoreTextObj.text = serverCount.ToString();
        }
    }



    public bool requireUpdate => true;

    public void OnUpdate()
    {
        timeLeft -= Time.deltaTime;
        timeTextObj.text = ((int)timeLeft).ToString();

        if (timeLeft <= 0)
        {
            timesUpTextObj.gameObject.SetActive(true);
            timesUpTextObj.text = "Time's Up!\nScore = " + serverCount.ToString();

            Destroy(Hand.Left.interactionController.gameObject);
            Destroy(Hand.Right.interactionController.gameObject);

            StartCoroutine(ReloadScene());
        }
    }


    private IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
