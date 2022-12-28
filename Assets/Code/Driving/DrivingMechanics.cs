using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class DrivingMechanics : Mechanics
{
    private bool CheckAcceleration()
    {
        return CheckThrottle(_RightHandTrigger);
    }

    private bool CheckBreak()
    {
        return CheckThrottle(_LeftHandTrigger);
    }

    private void Update()
    {
        _IsOnThrottalL = CheckBreak();
        _IsOnThrottalR = CheckAcceleration();

    }

    private void FixedUpdate()
    {
       
    }
}
