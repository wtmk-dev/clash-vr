using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;
    public float AliveTime = 0f;

    public float FireForce => _FireForce;
    public float Damage => _ShotDamage;
    
    public void Fire(Vector3 shotDirection)
    {
        SetActive(true);
        _Rig.AddForce(shotDirection * FireForce, ForceMode.Impulse);
        AliveTime = 3f;
    }

    private Vector3 _ShotSize = new Vector3(0.1f, 0.3f, 1f);
    public void SetShotSize(float shotSize)
    {
        _ShotSize.x = shotSize;
    }

    public void Return()
    {
        OnReturnRequest?.Invoke(this);
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void SetShotDamage(float shotDamage)
    {
        _ShotDamage = shotDamage;
    }

    public void SetShotVelocity(float shotVelocity)
    {
        _FireForce = shotVelocity;
    }

    private Rigidbody _Rig;
    private Collider _Collider;
    private float _ShotDamage = 10f, _FireForce = 37f;

    void Awake()
    {
        _Rig = GetComponent<Rigidbody>();
        _Collider = GetComponent<Collider>();
    }

    void Update()
    {
        if(AliveTime > 0)
        {
            AliveTime -= Time.deltaTime;
        }

        if(AliveTime <= 0)
        {
            SetActive(false);
            Return();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
    }

}

