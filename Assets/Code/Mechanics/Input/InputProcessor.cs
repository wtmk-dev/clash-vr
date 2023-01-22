using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InputProcessor : MonoBehaviour
{
    [SerializeField]
    private ActionBasedController _Controller;
    [SerializeField]
    private Animator _Hand;

    private float _GripValue;
    private float _TriggerValue;
    
    private void Update()
    {
        if(_Controller == null)
        {
            return;
        }

        _GripValue = _Controller.selectAction.action.ReadValue<float>();
        _TriggerValue = _Controller.activateAction.action.ReadValue<float>();

        _Hand.SetFloat("Grip", _GripValue);
        _Hand.SetFloat("Trigger", _TriggerValue);
    }

}
