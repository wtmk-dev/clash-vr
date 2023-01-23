using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VR_Button : MonoBehaviour
{
    public event Action OnPress;
    public event Action OnRelease;

    private float _Up;
    private float _Down;

    [SerializeField]
    private Triggerable _Triggerable;
    [SerializeField]
    private int _Trigger = 0;

    private void Awake()
    {
        _Up = transform.position.y;
        _Down = transform.position.y - .01f;
    }

    public void OnHoverEnter()
    {
        Debug.Log("Hovering");
        transform.DOMoveY(_Down, 0.1f);
        _Triggerable.Trigger(_Trigger);
    }

    public void OnHoverExit()
    {
        Debug.Log("Hovering Exit");
        transform.DOMoveY(_Up, 0.1f);
    }
}
