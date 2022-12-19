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
    private InputActionAsset _Input;
    [SerializeField]
    private Transform _LeftTracking;

    [SerializeField]
    private FuelTank _FuelTank;

    private Rigidbody _Rig;

    private bool _Boostable = true, _CanUseThrottal = true;
    private bool _IsBoosting, _IsOnThrottal, _ReFueling;

    private float _FuelCost = .01f;
    private float _Acceleration = 15f;
    private float _CurrentAcceleration = 15f;
    private float _MaxAcceleration = 100f;
    private float _Speed = 10f;
    private float _BoostPower = 600f;

    private InputActionMap _LeftHand;

    private InputAction _LeftHandPosition;
    private Vector2 _LeftPositionValue;

    private InputAction _LeftHandTrigger;
    private float _LeftTriggerValue;

    [SerializeField]
    private InputActionProperty _LeftTrigger;
    


    private void Awake()
    {
        _Rig = GetComponent<Rigidbody>();

        _FuelTank.OnEmpty += OnEmpty;
        _FuelTank.OnHasFuel += OnHasFuel;
    }

    private void _LeftHandPosition_performed(InputAction.CallbackContext obj)
    {
        _LeftPositionValue = obj.ReadValue<Vector2>();
        //Debug.Log(_LeftPositionValue);
    }

    private void _LeftHandTrigger_performed(InputAction.CallbackContext obj)
    {
        _LeftTriggerValue = obj.ReadValue<float>();

        Debug.Log(_LeftTriggerValue);
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

    private void OnLeftTriggerPressed(InputAction.CallbackContext obj)
    {
        var isActive = obj.ReadValue<bool>();
        Debug.Log($"Left triggered pressed {isActive}");
    }

    private void Update()
    {
        _LeftTriggerValue = _LeftTrigger.action.ReadValue<float>();
        Debug.Log($"Trigger value {_LeftTriggerValue}");

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
        var space = Keyboard.current.spaceKey;

        if (_CanUseThrottal)
        {
               if(space.IsPressed())
            {
                _ReFueling = false;
                _IsOnThrottal = true;
                ResetCurrentAcceleration();
            }
        }
        
        if(!space.IsPressed())
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
            var q = Keyboard.current.qKey;
            if (q.wasPressedThisFrame)
            {
                //Debug.Log("was pressed this frame");
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
                //Debug.Log(_CurrentAcceleration);
                //Debug.Log(_Rig.velocity);
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
            _Rig.AddForce(Vector3.up * _BoostPower, ForceMode.Force);
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
