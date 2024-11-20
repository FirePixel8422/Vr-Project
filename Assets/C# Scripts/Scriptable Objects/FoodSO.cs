using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Food", menuName = "Cooking System/Food", order = 0)]
public class FoodSO : MonoBehaviour
{
    public FoodType foodType;
}



[System.Serializable]
public struct FoodType
{
    public string foodName;
}