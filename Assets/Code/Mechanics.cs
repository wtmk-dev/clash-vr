using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Mechanics : MonoBehaviour
{
    [Space]
    [SerializeField]
    protected InputActionReference _LeftHandTrigger, _LeftHandGrip;
    [Space]
    [SerializeField]
    protected InputActionReference _RightHandTrigger, _RightHandGrip;
    [Space]
    [SerializeField]
    protected XRController _LeftXRController, _RightXRController;

    [SerializeField]
    protected Rigidbody _Rig;

    protected XRInputProcessor _XRInputProcessor = new XRInputProcessor();
    protected virtual bool CheckTrigger(InputActionReference inputRef)
    {
        var isOnThrottle = false;
        var trigger = inputRef.action.ReadValue<float>();
        isOnThrottle = trigger > .01f;
        return isOnThrottle;
    }
}
