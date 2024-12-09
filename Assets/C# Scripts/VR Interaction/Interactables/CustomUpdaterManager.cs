using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;


[BurstCompile]
public class CustomUpdaterManager : MonoBehaviour
{
    public static CustomUpdaterManager Singleton { get; private set; }

    //extra Instance name for simplicity
    public static CustomUpdaterManager Instance => Singleton;

    [BurstCompile]
    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Singleton = this;

        updateStack = new List<ICustomUpdater>(updateListPreSizeCap);
        lateUpdateStack = new List<ICustomLateUpdater>(updateListPreSizeCap);
        update_10FPSStack = new List<ICustomIntervalUpdater_10FPS>(updateListPreSizeCap);

        StartCoroutine(UpdateLoop());
    }





    public static List<ICustomUpdater> updateStack;
    public static List<ICustomLateUpdater> lateUpdateStack;
    public static List<ICustomIntervalUpdater_10FPS> update_10FPSStack;
    public int updateListPreSizeCap;


    public static void AddUpdater(ICustomUpdater newEntry)
    {
        updateStack.Add(newEntry);
    }
    public static void AddUpdater(ICustomLateUpdater newEntry)
    {
        lateUpdateStack.Add(newEntry);
    }
    public static void AddUpdater(ICustomIntervalUpdater_10FPS newEntry)
    {
        update_10FPSStack.Add(newEntry);
    }


    [BurstCompile]
    private IEnumerator UpdateLoop()
    {
        float elapsedTime = 0;

        while (true)
        {
            yield return null;

            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 0.2f)
            {
                Update_IntervalUpdateStack_10FPS(elapsedTime);

                elapsedTime = 0;
            }



            Update_UpdateStack();

            yield return new WaitForEndOfFrame();

            Update_LateUpdateStack();
        }
    }


    [BurstCompile]
    private void Update_UpdateStack()
    {
        for (int i = 0; i < updateStack.Count; i++)
        {
            if (updateStack[i].requireUpdate)
            {
                updateStack[i].OnUpdate();
            }
        }
    }

    [BurstCompile]
    private void Update_LateUpdateStack()
    {
        for (int i = 0; i < lateUpdateStack.Count; i++)
        {
            if (lateUpdateStack[i].requireLateUpdate)
            {
                lateUpdateStack[i].OnLateUpdate();
            }
        }
    }

    [BurstCompile]
    private void Update_IntervalUpdateStack_10FPS(float deltaTime)
    {
        for (int i = 0; i < update_10FPSStack.Count; i++)
        {
            if (update_10FPSStack[i].requireUpdate_10FPS)
            {
                update_10FPSStack[i].OnIntervalUpdate_10FPS(deltaTime);
            }
        }
    }
}
