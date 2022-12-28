using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInputProcessor
{
    public Vector3 GetDecivePosition(XRNode node)
    {
        var outValue = new Vector3();

        if (!_Devices.ContainsKey(node))
        {
            var inputDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(node, inputDevices);
            var device = inputDevices[0];
            _Devices.Add(node, device);
        }

        _Devices[node].TryGetFeatureValue(UnityEngine.XR.CommonUsages.devicePosition, out outValue);
        return outValue;
    }

    public Quaternion GetDeviceRotation(XRNode node)
    {
        var outValue = new Quaternion();

        if (!_Devices.ContainsKey(node))
        {
            var inputDevices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(node, inputDevices);
            var device = inputDevices[0];
            _Devices.Add(node, device);
        }

        _Devices[node].TryGetFeatureValue(UnityEngine.XR.CommonUsages.deviceRotation, out outValue);
        return outValue;
    }
    private Dictionary<XRNode, InputDevice> _Devices = new Dictionary<XRNode, InputDevice>();
}
