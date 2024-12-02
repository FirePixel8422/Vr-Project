using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Flags]
public enum TurnInteractableRotationMode : byte
{
    X = 1,
    Y = 2,
    Z = 4,
}

public class TurnInteractable : Interactable
{
    [Header("")]

    public Transform hinge;

    public TurnInteractableRotationMode turnMode;
}
