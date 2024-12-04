using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;


public interface ICustomLateUpdater
{
    public bool requireLateUpdate { get; }

    [BurstCompile]
    public void OnLateUpdate();
}
