using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class FlyingMechanics : Mechanics
{
    public void SetBoostable(bool isActive)
    {
        _Boostable = isActive;
    }

    private XRNode _HeadNode = XRNode.Head, _RightHandNode = XRNode.RightHand, _LeftHandNode = XRNode.LeftHand;
    private Vector3 _LeftHandPos, _RightHandPos, _HeadPos;
    private Quaternion _LeftHandRot, _RightHandRot, _HeadRot;

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
        GetHeadPosition();
        GetLeftHandPosition();
        GetRightHandPosition();

        CheckBoost();

        DoRefill();

        _IsOnThrottalL = CheckThrottle(_LeftHandTrigger);
        _IsOnThrottalR = CheckThrottle(_RightHandTrigger);
    }

    private void LateUpdate()
    {
        transform.rotation = new Quaternion(transform.rotation.x, 0f, transform.rotation.z, transform.rotation.z);
    }

    private void GetLeftHandPosition()
    {
        var leftXRPosition = _LeftXRController.transform;
        _LeftHandPos = leftXRPosition.forward;
    }

    private void GetRightHandPosition()
    {
        var leftXRPosition = _RightXRController.transform;
        _RightHandPos = leftXRPosition.forward;
    }

    private void GetHeadPosition()
    {
        _HeadPos = _XRInputProcessor.GetDecivePosition(_HeadNode);
    }

    private void FixedUpdate()
    {
        DoBoost();
        DoAccelerate(_LeftHandPos, _IsOnThrottalL);
        DoAccelerate(_RightHandPos, _IsOnThrottalR);
    }

    private void DoAccelerate(Vector3 handPos, bool isOnThrottal)
    {
        if(isOnThrottal)
        {
            if(_CurrentAcceleration < _MaxAcceleration)
            {
                var reverseVector = handPos.normalized;
                reverseVector *= -1;

                _Rig.AddForce(reverseVector * (_CurrentAcceleration + (_Speed * Time.deltaTime)), ForceMode.Acceleration);
            }
        }
    }

    private void CheckBoost()
    {
        if (_Boostable)
        {
            var boost = _LeftHandGrip.action.WasPressedThisFrame();
            if (boost)
            {
                _Boostable = false;
                _IsBoosting = true;
            }
        }
    }

    private void DoBoost()
    {
        if (_IsBoosting)
        {
            _IsBoosting = false;

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

}
