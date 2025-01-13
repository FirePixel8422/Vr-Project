using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInitializeInOrder
{
    public int InitOrder { get; set; }

    public void Init();
}