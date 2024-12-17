using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class FridgeLight : TurnInteractable
{
    [SerializeField] private float rotForLight;

    [Header("true for More Then RotForLight, false for less")]
    [SerializeField] private bool moreLight;

    [SerializeField] private Transform rotCheckTransform;

    [SerializeField] private bool lightState;

    [SerializeField] private Light[] lights;



    protected override void Start()
    {
        base.Start();

        ToggleLights(false);
    }


    protected override void RotateTransform(Vector3 transformPos, Vector3 handTransformPos)
    {
        base.RotateTransform(transformPos, handTransformPos);

        bool newLightState = false;

        if (moreLight)
        {
            if (rotCheckTransform.localEulerAngles.y > rotForLight)
            {
                newLightState = true;
            }
        }
        else
        {
            if (rotCheckTransform.localEulerAngles.y < rotForLight)
            {
                newLightState = true;
            }
        }


        if (newLightState != lightState)
        {
            lightState = newLightState;

            ToggleLights (newLightState);
        }
    }


    [BurstCompile]
    private void ToggleLights(bool state)
    {
        int lightCount = lights.Length;

        for (int i = 0; i < lightCount; i++)
        {
            lights[i].enabled = state;
        }
    }
}
