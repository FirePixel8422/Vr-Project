using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "New Applience", menuName = "Cooking System/Applience", order = 0)]
public class ApplienceSO : ScriptableObject
{
    public string applienceName;


    private void OnValidate()
    {
        applience.applienceName = applienceName;
    }

    [HideInInspector]
    public Applience applience;
}



[System.Serializable]
public struct Applience : IEquatable<Applience>
{
    public FixedString128Bytes applienceName;


    public bool Equals(Applience other)
    {
        return applienceName == other.applienceName;
    }


    public override bool Equals(object obj)
    {
        if (obj is Applience other)
        {
            return Equals(other);
        }
        return false;
    }


    public override int GetHashCode()
    {
        return applienceName.ToString()?.GetHashCode() ?? 0;
    }


    public static bool operator ==(Applience lhs, Applience rhs)
    {
        return lhs.Equals(rhs);
    }


    public static bool operator !=(Applience lhs, Applience rhs)
    {
        return !lhs.Equals(rhs);
    }
}