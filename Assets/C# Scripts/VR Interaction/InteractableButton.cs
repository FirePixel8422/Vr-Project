using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : Interactable
{
    private Animator buttonAnimator;

    public virtual void Start()
    {
        buttonAnimator = GetComponent<Animator>();
    }


    public override void Pickup(InteractionController hand)
    {
        buttonAnimator.SetTrigger("Press");
    }
}
