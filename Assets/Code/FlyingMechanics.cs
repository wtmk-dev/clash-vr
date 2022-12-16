using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyingMechanics : MonoBehaviour
{
    public void SetBoostable(bool isActive)
    {
        _Boostable = isActive;
    }

    [SerializeField]
    private InputActionAsset _Input;

    [SerializeField]
    private InputActionReference _LeftHandPosition;
    [SerializeField]
    private InputActionReference _LeftTriggerState;

    private Rigidbody _Rig;
    private InputActionMap _LeftHand;

    private bool _Boostable = true;

    private void Awake()
    {
        _Rig = GetComponent<Rigidbody>();
        _LeftTriggerState.action.performed += OnLeftTriggerPressed;
    }

    private void OnLeftTriggerPressed(InputAction.CallbackContext obj)
    {
        var isActive = obj.ReadValue<bool>();
        Debug.Log($"Left triggered pressed {isActive}");
    }

    private void Update()
    {
        var val = _LeftHandPosition.action.ReadValue<Vector3>();
        Debug.Log("Left hand pos: " + val);

        CheckBoost();
    }

    private void CheckBoost()
    {
        if(_Boostable)
        {
            var q = Keyboard.current.qKey;
            if (q.wasPressedThisFrame)
            {
                Debug.Log("was pressed this frame");
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
            _Rig.AddForce(Vector3.up * 15f, ForceMode.Force);
        }
    }

    private void FixedUpdate()
    {
        DoBoost();
    }

    private bool _IsBoosting;
}
