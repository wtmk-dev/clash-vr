using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mob : MonoBehaviour, IPoolable
{
    [SerializeField]
    private MobType _Type;
    [SerializeField]
    private float FireForce = 10;

    public void Spawn(Transform target)
    {
        SetActive(true);
        gameObject.SetActive(true);
        _Rig.AddForce(-target.forward * FireForce, ForceMode.Impulse);
    }

    private Rigidbody _Rig;

    public event Action<IPoolable> OnReturnRequest;

    private void Awake()
    {
        _Rig = GetComponent<Rigidbody>();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Return()
    {
        OnReturnRequest?.Invoke(this);
    }
}
