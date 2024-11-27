using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;


public class Hand : MonoBehaviour
{
    #region Hands static Instances Setup

    public static Hand Left;
    public static Hand Right;

    private void Awake()
    {
        if (handType == HandType.Left)
        {
            Left = this;
        }
        else
        {
            Right = this;
        }
    }

    public HandType handType;
    public enum HandType
    {
        Left,
        Right,
    };

    #endregion

    private InteractionManager IM;

    private InteractionController interactionController;

    private HapticImpulsePlayer hapticImpulsePlayer;


    private void Start()
    {
        IM = InteractionManager.Instance;

        interactionController = GetComponent<InteractionController>();

        hapticImpulsePlayer = GetComponent<HapticImpulsePlayer>();
    }



    [BurstCompile]
    public void SendVibration(float amplitude, float duration, float frequency)
    {
        hapticImpulsePlayer.SendHapticImpulse(amplitude, duration, frequency);
    }
    [BurstCompile]
    public void SendVibration(VibrationParamaters vibrationParams)
    {
        hapticImpulsePlayer.SendHapticImpulse(vibrationParams.amplitude, vibrationParams.duration, vibrationParams.frequency);
    }
    [BurstCompile]
    public void SendPickupVibration()
    {
        hapticImpulsePlayer.SendHapticImpulse(IM.pickupVibrationParams.amplitude, IM.pickupVibrationParams.duration, IM.pickupVibrationParams.frequency);
    }
}
