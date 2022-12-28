using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Mechanics : MonoBehaviour
{
    [SerializeField]
    protected FuelTank _FuelTank;
    [Space]
    [SerializeField]
    protected InputActionReference _LeftHandTrigger, _LeftHandGrip;
    [Space]
    [SerializeField]
    protected InputActionReference _RightHandTrigger, _RightHandGrip;
    [Space]
    [SerializeField]
    protected XRController _LeftXRController, _RightXRController;

    protected Rigidbody _Rig;

    protected bool _Boostable = true, _CanUseThrottal = true;
    protected bool _IsBoosting, _IsOnThrottalL, _IsOnThrottalR, _ReFueling;

    protected float _FuelCost = .01f;
    protected float _Acceleration = 15f;
    protected float _CurrentAcceleration = 15f;
    protected float _MaxAcceleration = 200f;
    protected float _Speed = 100f;
    protected float _BoostPower = 100f;
    protected XRInputProcessor _XRInputProcessor = new XRInputProcessor();

    protected virtual void ResetCurrentAcceleration()
    {
        if (_CurrentAcceleration < _Acceleration)
        {
            _CurrentAcceleration = _Acceleration;
        }
    }

    protected virtual bool CheckThrottle(InputActionReference trigger)
    {
        var isOnThrottle = false;
        var throttle = trigger.action.ReadValue<float>();
        //Debug.Log(throttle);

        var isPressed = false;

        isPressed = throttle > .01f;
        //isPressed = Keyboard.current.spaceKey.IsPressed();

        if (_CanUseThrottal)
        {
            if (isPressed)
            {
                _ReFueling = false;
                isOnThrottle = true;
                ResetCurrentAcceleration();
            }
        }

        if (!isPressed)
        {
            isOnThrottle = false;
            _ReFueling = true;
            if (_CurrentAcceleration > _Acceleration)
            {
                _CurrentAcceleration -= (_Speed * Time.deltaTime);
            }
        }

        return isOnThrottle;
    }


    protected virtual void DoRefill()
    {
        if (_ReFueling)
        {
            _FuelTank.Fill(_FuelCost);
        }
    }

}
