using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class DrivingMechanics : Mechanics
{
    [SerializeField]
    private Transform _Move;
    private DrivingMechanicsDynamics _Dynamics;
    private DrivingMechanicsModel _Model;

    private void Awake()
    {
        _Dynamics = new DrivingMechanicsDynamics();
        _Model = new DrivingMechanicsModel();
    }

    private void Update()
    {
        _Dynamics.IsOnBreak = CheckBreak();
        _Dynamics.IsOnAccelerator = CheckAcceleration();
        _Dynamics.IsOnLeftGrip = CheckTrigger(_LeftHandGrip);
        _Dynamics.IsOnRightGrip = CheckTrigger(_RightHandGrip);
    }

    private void FixedUpdate()
    {
        DoRotateAroundAxis(_Dynamics);
        //DoBreak(_Dynamics.IsOnBreak);

        //DoSide(_Dynamics.IsOnLeftGrip, -_LeftHandGrip.action.ReadValue<float>());
        //DoSide(_Dynamics.IsOnRightGrip, _RightHandGrip.action.ReadValue<float>());
        DoAccelerate(_Dynamics.IsOnAccelerator, _RightHandTrigger.action.ReadValue<float>());
        DoAccelerate(_Dynamics.IsOnBreak, -_LeftHandTrigger.action.ReadValue<float>());
    }

    private bool CheckAcceleration()
    {
        return CheckTrigger(_RightHandTrigger);
    }

    private bool CheckBreak()
    {
        return CheckTrigger(_LeftHandTrigger);
    }

    private bool CheckLeftTurn()
    {
        return CheckTrigger(_LeftHandGrip);
    }

    private bool CheckRightTurn()
    {
        return CheckTrigger(_RightHandGrip);
    }

    private void DoAccelerate(bool isOnThrottal, float speed)
    {
        if (isOnThrottal)
        {
            var moveVector = new Vector3(0f, 0f, speed);
            Debug.Log(moveVector);

            _Rig.AddForce(moveVector * (100f + 
                         (_Model.Foce * Time.deltaTime)), ForceMode.Impulse);

            //_Rig.MovePosition(transform.position + moveVector * Time.deltaTime * _Model.Foce);
        }
        else
        {
            _Rig.velocity = Vector3.zero;
        }
    }

    private void DoSide(bool isOnThrottal, float speed)
    {
        if (isOnThrottal)
        {
            var moveVector = new Vector3(speed, 0f, 0f);
            Debug.Log(moveVector);

            _Rig.AddForce(moveVector * (100f + 
                         (_Model.Foce * Time.deltaTime)), ForceMode.Impulse);

            //_Rig.MovePosition(transform.position + moveVector * Time.deltaTime * _Model.Foce);
        }
    }

    private void DoBreak(bool isOnThrottal)
    {
        if (isOnThrottal)
        {
            if (_Dynamics.CurrentAcceleration > 0f)
            {
                _Rig.velocity = Vector3.zero;
                _Dynamics.CurrentAcceleration -= Time.deltaTime;
            }
        }
    }

    private void DoRotateAroundAxis(DrivingMechanicsDynamics dynamics)
    {
        float degree = 0f;
        if(CheckLeftTurn())
        {
            degree = -_LeftHandGrip.action.ReadValue<float>();
        }else if(CheckRightTurn())
        {
            degree = _RightHandGrip.action.ReadValue<float>();
        }
        else
        {
            degree = 0f;
        }

        degree *= 45f;
        //Debug.Log(degree);

        dynamics.RotateSpeed = _Rig.velocity.magnitude;
        dynamics.AngularSpeed = _Rig.angularVelocity.magnitude * Mathf.Rad2Deg;

        var rotateAngle = Quaternion.AngleAxis(degree, Vector3.up);

        float angle;
        Vector3 axis;

        rotateAngle.ToAngleAxis(out angle, out axis);
        _Rig.angularVelocity = axis * angle * Mathf.Deg2Rad;
    }

}

public class DrivingMechanicsDynamics
{
    public bool IsOnLeftGrip { get; set; }
    public bool IsOnRightGrip { get; set; }
    public bool IsOnBreak { get; set; }
    public bool IsOnAccelerator { get; set; }
    public float RotateSpeed { get; set; }
    public float AngularSpeed { get; set; }
    public float CurrentAcceleration { get; set; }
}

public class DrivingMechanicsModel
{
    public float Foce => _Force;
    private float _Force = 100f;
    public float BreakFoce => _BreakForce;
    private float _BreakForce = 1f;
    public float Acceleration => _Acceleration;
    private float _Acceleration = .15f;
    public float MaxAcceleration => _MaxAcceleration;
    private float _MaxAcceleration = 15f;
}

