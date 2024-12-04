using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;


[BurstCompile]
public class InteractableUpdateManager : MonoBehaviour
{
    public static InteractableUpdateManager Singleton { get; private set; }

    //extra Instance name for simplicity
    public static InteractableUpdateManager Instance => Singleton;

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
    }





    public static List<ICustomUpdater> updateStack;
    public static List<ICustomLateUpdater> lateUpdateStack;
    public int updateListPreSizeCap;


    public static void AddUpdater(ICustomUpdater newEntry)
    {
        updateStack.Add(newEntry);
    }
    public static void AddUpdater(ICustomLateUpdater newEntry)
    {
        lateUpdateStack.Add(newEntry);
    }



    [BurstCompile]
    private void Update()
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
    private void LateUpdate()
    {
        for (int i = 0; i < lateUpdateStack.Count; i++)
        {
            if (lateUpdateStack[i].requireLateUpdate)
            {
                lateUpdateStack[i].OnLateUpdate();
            }
        }
    }
}
