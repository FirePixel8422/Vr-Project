using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRHandAnimator : MonoBehaviour, ICustomUpdater
{
    private Animator anim;

    private float controllerButtonPressPercent;
    private float _cButtonPressPercent;
    public float valueUpdateSpeed;

    private Vector3 localPos;
    private Quaternion localRot;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnBigTriggerStateChange(InputAction.CallbackContext ctx)
    {
        controllerButtonPressPercent = ctx.ReadValue<float>();
    }


    public bool requireUpdate => _cButtonPressPercent == controllerButtonPressPercent;

    public void OnUpdate()
    {
        _cButtonPressPercent = Mathf.MoveTowards(_cButtonPressPercent, controllerButtonPressPercent, valueUpdateSpeed * Time.deltaTime);
        anim.SetFloat("GrabStrength", _cButtonPressPercent);
    }



    public void UpdateHandTransform(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }

    public void ResetHandTransform()
    {
        transform.SetLocalPositionAndRotation(localPos, localRot);
    }
}
