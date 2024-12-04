using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;


public interface IInteractableUpdater
{
    public bool requireUpdate { get; }

    [BurstCompile]
    public void OnUpdate();
}
