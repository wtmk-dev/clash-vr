using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;


public class FlyingMechanics : MonoBehaviour
{
    public void SetBoostable(bool isActive)
    {
        _Boostable = isActive;
    }

    [SerializeField]
    private Transform _LeftTracking;

    [SerializeField]
    private FuelTank _FuelTank;

    private Rigidbody _Rig;

    private bool _Boostable = true, _CanUseThrottal = true;
    private bool _IsBoosting, _IsOnThrottal, _ReFueling;

    private float _FuelCost = .001f;
    private float _Acceleration = 15f;
    private float _CurrentAcceleration = 15f;
    private float _MaxAcceleration = 200f;
    private float _Speed = 10f;
    private float _BoostPower = 100f;

    [SerializeField]
    private InputActionReference _LeftHandPosition;
    private Vector2 _LeftPositionValue;

    [SerializeField]
    private InputActionReference _LeftHandTrigger, _LeftHandGrip;
    private float _LeftTriggerValue;


    private void Awake()
    {
        _Rig = GetComponent<Rigidbody>();       

        _FuelTank.OnEmpty += OnEmpty;
        _FuelTank.OnHasFuel += OnHasFuel;
    }

    private void OnEmpty()
    {
        _CanUseThrottal = false;
        _IsOnThrottal = false;
        _Rig.velocity = Vector3.zero;
    }

    private void OnHasFuel()
    {
        _CanUseThrottal = true;
    }

    private void Update()
    {
        CheckBoost();
        CheckThrottle();
        DoRefill();
    }

    private void FixedUpdate()
    {
        DoBoost();
        DoAccelerate();
    }

    private void CheckThrottle()
    {
        var throttle = _LeftHandTrigger.action.ReadValue<float>();
        Debug.Log(throttle);

        var isPressed = false;

        //isPressed = throttle > 0f && throttle < 1f ? true : false;
        isPressed = Keyboard.current.spaceKey.IsPressed();

        if (_CanUseThrottal)
        {
            if(isPressed)
            {
                _ReFueling = false;
                _IsOnThrottal = true;
                ResetCurrentAcceleration();
            }
        }
        
        if(!isPressed)
        {
            _IsOnThrottal = false;
            _ReFueling = true;
            if (_CurrentAcceleration > _Acceleration)
            {
                _CurrentAcceleration -= (_Speed * Time.deltaTime);
            }
        }
    }

    private void CheckBoost()
    {
        if(_Boostable)
        {
            //var boost = _LeftHandGrip.action.WasPressedThisFrame();
            var boost = Keyboard.current.qKey.wasPressedThisFrame;
            if (boost)
            {
                _IsBoosting = true;
            }
        }
    }

    private void DoAccelerate()
    {
        if(_IsOnThrottal)
        {
            if(_CurrentAcceleration < _MaxAcceleration)
            {
                _Rig.AddForce(Vector3.up * (_CurrentAcceleration + (_Speed * Time.deltaTime)), ForceMode.Acceleration);

                _FuelTank.Deplete(_FuelCost);
            }
        }
    }

    private void DoBoost()
    {
        if (_IsBoosting)
        {
            _IsBoosting = false;
            ResetCurrentAcceleration();
            _Rig.velocity = Vector3.zero;
            _Rig.AddForce(Vector3.up * _BoostPower, ForceMode.Impulse);
        }
    }

    private void DoRefill()
    {
        if (_ReFueling)
        {
            _FuelTank.Fill(_FuelCost);
        }
    }

    private void ResetCurrentAcceleration()
    {
        if (_CurrentAcceleration < _Acceleration)
        {
            _CurrentAcceleration = _Acceleration;
        }
    }    

}
