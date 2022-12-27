using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class FlyingMechanics : MonoBehaviour
{
    public void SetBoostable(bool isActive)
    {
        _Boostable = isActive;
    }

    [SerializeField]
    private FuelTank _FuelTank;

    private Rigidbody _Rig;

    private bool _Boostable = true, _CanUseThrottal = true;
    private bool _IsBoosting, _IsOnThrottalL, _IsOnThrottalR, _ReFueling;

    private float _FuelCost = .01f;
    private float _Acceleration = 15f;
    private float _CurrentAcceleration = 15f;
    private float _MaxAcceleration = 200f;
    private float _Speed = 100f;
    private float _BoostPower = 100f;
    
    private XRNode _HeadNode = XRNode.Head, _RightHandNode = XRNode.RightHand, _LeftHandNode = XRNode.LeftHand;
    private Vector3 _LeftHandPos, _RightHandPos, _HeadPos;
    private Quaternion _LeftHandRot, _RightHandRot, _HeadRot;

    [Space]
    [SerializeField]
    private InputActionReference _LeftHandTrigger, _LeftHandGrip;
    [Space]
    [SerializeField]
    private InputActionReference _RightHandTrigger, _RightHandGrip;
    [SerializeField]
    private XRController _LeftXRController, _RightXRController;

    private void Awake()
    {
        _Rig = GetComponent<Rigidbody>();       

        _FuelTank.OnEmpty += OnEmpty;
        _FuelTank.OnHasFuel += OnHasFuel;
        
    }

    private void OnEmpty()
    {
        _CanUseThrottal = false;
        _IsOnThrottalL = false;
        _IsOnThrottalR = false;

        _Rig.velocity = Vector3.zero;
    }

    private void OnHasFuel()
    {
        _CanUseThrottal = true;

        if(_FuelTank.CurrentValue >= 1f)
        {
            _Boostable = true;
        }
    }

    private void Update()
    {
        //CheckBoost();
        _IsOnThrottalL = CheckThrottle(_LeftHandTrigger);
        _IsOnThrottalR = CheckThrottle(_RightHandTrigger);
        DoRefill();
        GetLeftHandPosition();
        GetHeadPosition();
        GetRightHandPosition();
    }

    private void LateUpdate()
    {
        transform.rotation = new Quaternion(transform.rotation.x, 0f, transform.rotation.z, transform.rotation.z);
    }

    private void GetLeftHandPosition()
    {
        var leftXRPosition = _LeftXRController.transform;
        Debug.Log("l vector " + leftXRPosition.forward);

        _LeftHandPos = leftXRPosition.forward;//UpdateDevicePositions(_LeftHandNode);
    }

    private void GetRightHandPosition()
    {
        var leftXRPosition = _RightXRController.transform;
        Debug.Log("Right vector " + leftXRPosition.forward);

        _RightHandPos = leftXRPosition.forward;//UpdateDevicePositions(_RightHandNode);
    }

    private void GetHeadPosition()
    {
        _HeadPos = UpdateDevicePositions(_HeadNode);
    }

    private Vector3 UpdateDevicePositions(XRNode node)
    {
        var outValue = new Vector3();
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(node, inputDevices);

        var device = inputDevices[0];

        device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out outValue);
        return outValue;
    }

    private Quaternion UpdateDeviceRotation(XRNode node)
    {
        var outValue = new Quaternion();
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(node, inputDevices);

        var device = inputDevices[0];

        device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out outValue);
        return outValue;
    }

    private void FixedUpdate()
    {
        DoBoost();
        DoAccelerate(_LeftHandPos, _IsOnThrottalL);
        DoAccelerate(_RightHandPos, _IsOnThrottalR);
    }

    private bool CheckThrottle(InputActionReference trigger)
    {
        var isOnThrottle = false;
        var throttle = trigger.action.ReadValue<float>();
        //Debug.Log(throttle);

        var isPressed = false;

        isPressed = throttle > .01f;
        //isPressed = Keyboard.current.spaceKey.IsPressed();

        if (_CanUseThrottal)
        {
            if(isPressed)
            {
                _ReFueling = false;
                isOnThrottle = true;
                ResetCurrentAcceleration();
            }
        }
        
        if(!isPressed)
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

    private void CheckBoost()
    {
        if(_Boostable)
        {
            var boost = _LeftHandGrip.action.WasPressedThisFrame();
            //var boost = Keyboard.current.qKey.wasPressedThisFrame;
            if (boost)
            {
                _Boostable = false;
                _IsBoosting = true;
            }
        }
    }


    private void DoAccelerate(Vector3 handPos, bool isOnThrottal)
    {
        if(isOnThrottal)
        {
            if(_CurrentAcceleration < _MaxAcceleration)
            {
                Debug.Log("Hand " + handPos);
                Debug.Log("Head " + _HeadPos);

                var reverseVector = (handPos - _HeadPos).normalized;
                reverseVector *= -1;

                Debug.Log("ending " + reverseVector);
                _Rig.AddForce(reverseVector * (_CurrentAcceleration + (_Speed * Time.deltaTime)), ForceMode.Acceleration);
            }
        }
    }

    private void DoBoost()
    {
        if (_IsBoosting)
        {
            _IsBoosting = false;
           //Debug.Log(_FuelTank.CurrentValue);
            if (_FuelTank.CurrentValue > 0.1 && _FuelTank.CurrentValue < 0.18)
            {
                Debug.Log("crit");
                _Boostable = true;
                _FuelTank.Fill(1);
            }
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
