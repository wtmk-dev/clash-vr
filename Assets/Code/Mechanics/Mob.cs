using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Mob : MonoBehaviour, IPoolable
{
    public event Action<IPoolable> OnReturnRequest;

    [SerializeField]
    private MobType _Type;
    [SerializeField]
    private float Speed = 3f;
    [SerializeField]
    private float AliveTime = 1.3f;

    private bool _IsMoving = false;
    private Transform _CurrentTarget;
    private DeltaTimeActiveCallback _Return;

    public void Spawn(Transform target)
    {
        SetActive(true);
        gameObject.SetActive(true);
        _CurrentTarget = target;
        _IsMoving = true;
        _Return.Start();
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void Return()
    {
        _IsMoving = false;
        SetActive(false);
        OnReturnRequest?.Invoke(this);
    }

    private void Awake()
    {
        _Rig = GetComponent<Rigidbody>();
        _Return = new DeltaTimeActiveCallback(AliveTime, Return);
    }

    private void Update()
    {
        _Return.Update();

        if (_IsMoving)
            MoveTo(_CurrentTarget);
    }

    private void MoveTo(Transform target)
    {
        // Move our position a step closer to the target.
        var step = Speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        // Check if the position of the cube and sphere are approximately equal.
        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            // Swap the position of the cylinder.
            target.position *= -1.0f;
        }
    }

    private Rigidbody _Rig;
}
