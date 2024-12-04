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

        updateStack = new List<IInteractableUpdater>(updateListPreSizeCap);
    }





    public static List<IInteractableUpdater> updateStack;
    public int updateListPreSizeCap;


    public static void AddUpdater(IInteractableUpdater newEntry)
    {
        updateStack.Add(newEntry);
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
}
