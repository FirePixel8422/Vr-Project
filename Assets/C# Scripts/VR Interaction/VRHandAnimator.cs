using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRHandAnimator : MonoBehaviour, ICustomUpdater
{
    private Animator anim;

    [SerializeField] private float controllerButtonPressPercent;
    [SerializeField] private float _cButtonPressPercent;
    public float valueUpdateSpeed;

    private Vector3 localPos;
    [HideInInspector]
    public Quaternion localRot;


    private void Start()
    {
        anim = GetComponent<Animator>();

        localPos = transform.localPosition;
        localRot = transform.localRotation;

        CustomUpdaterManager.AddUpdater(this);
    }

    public void OnBigTriggerStateChange(InputAction.CallbackContext ctx)
    {
        controllerButtonPressPercent = ctx.ReadValue<float>();
    }


    public bool requireUpdate => _cButtonPressPercent != controllerButtonPressPercent;

    public void OnUpdate()
    {
        _cButtonPressPercent = Mathf.MoveTowards(_cButtonPressPercent, controllerButtonPressPercent, valueUpdateSpeed * Time.deltaTime);
        anim.SetFloat("GrabStrength", _cButtonPressPercent);
    }



    public void UpdateHandTransform(Vector3 pos, Quaternion rot, bool flipHand)
    {
        Quaternion targetRot = rot;
        if (flipHand)
        {
            targetRot *= Quaternion.Euler(0, 0, 180);
        }

        Vector3 targetPos = pos;
        if (flipHand)
        {
            targetPos = new Vector3(-pos.x, pos.y, pos.z);
        }

        transform.SetPositionAndRotation(targetPos, targetRot);
    }

    public void ResetHandTransform()
    {
        transform.SetLocalPositionAndRotation(localPos, localRot);
    }
}
