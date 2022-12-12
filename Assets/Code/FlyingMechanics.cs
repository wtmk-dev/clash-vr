using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyingMechanics : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _Input;

    [SerializeField]
    private InputActionReference _LeftHandPosition;
    [SerializeField]
    private InputActionReference _LeftTriggerState;

    private Rigidbody _Rig;
    private InputActionMap _LeftHand;


    private void Awake()
    {
        _Rig = GetComponent<Rigidbody>();
        
        for (int i = 0; i < _Input.actionMaps.Count; i++)
        {
            Debug.Log(_Input.actionMaps[i]);
        }

        _LeftHand = _Input.actionMaps[1];
        var act = _LeftHand.FindAction("Position");

        _LeftTriggerState.action.performed += OnLeftTriggerPressed;
    }

    private void OnLeftTriggerPressed(InputAction.CallbackContext obj)
    {
        Debug.Log(obj.ReadValue<bool>());
    }

    private void Update()
    {
        var val = _LeftHandPosition.action.ReadValue<Vector3>();
        Debug.Log("Left hand pos: " + val);
    }

}
