using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Button : MonoBehaviour
{
    public event Action OnPress;
    public event Action OnRelease;

    public void OnHover()
    {
        Debug.Log("Hovering");
    }
}
