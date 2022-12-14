using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostRing : MonoBehaviour
{
    public event Action OnTriggered;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        OnTriggered?.Invoke();
    }
}
