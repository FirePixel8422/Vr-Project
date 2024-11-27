using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[Flags]
public enum GrabState : byte
{
    OnSphereTrigger = 1,
    OnRaycast = 2,
}


public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;
    private void Awake()
    {
        Instance = this;
    }



    [Header("What pickup methods to use")]
    public GrabState grabState = (GrabState)3;

    [Header("What layer should the interactables be on?")]
    public LayerMask interactablesLayer;

    [Header("Can your hand pickup an object in you other hand?")]
    public bool canSwapItemFromHands = true;

    [Header("Does a newly picked up object gain hand previous velocity?")]
    public bool pickupsUseOldHandVel;

    [Header("Raycast forward pickup range")]
    public float interactRayCastRange = 2;

    [Header("Distance to hand pickup range")]
    public float overlapSphereSize = .2f;

    [Header("Does not affect the velocity clamp of interactables")]
    public float throwVelocityMultiplier = 1.5f;

    [Header("Array Size of the OverlapSphere")]
    public int maxExpectedObjectInSphere = 15;

    [Header("Vibration of the controller when an object is picked up")]
    public VibrationParamaters pickupVibrationParams = new VibrationParamaters(1, 0, 0.2f);
}


[System.Serializable]
public struct VibrationParamaters
{
    public VibrationParamaters(float _amplitude, float _frequency, float _duration)
    {
        amplitude = _amplitude;
        frequency = _frequency;
        duration = _duration;
    }

    [Range(0f, 1f)]
    public float amplitude;

    public float frequency;
    
    public float duration;
}
