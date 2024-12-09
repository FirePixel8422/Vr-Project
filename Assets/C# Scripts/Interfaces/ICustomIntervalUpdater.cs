using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;


public interface ICustomIntervalUpdater_10FPS
{
    public bool requireUpdate_10FPS { get; }

    [BurstCompile]
    public void OnIntervalUpdate_10FPS(float deltaTime);
}
