using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Triggerable
{
    public Transform ShotSpawnPoint;
    [SerializeField]
    private GameObject _ShotPrefab;
    [SerializeField]
    private int _ShotPoolSize;

    public override void Trigger()
    {
        base.Trigger();
        Fire();
    }

    private Pool _ShotPool;
    private List<IPoolable> _Shots;

    private void Awake()
    {
        BuildShotPool();
    }

    private void Fire()
    {
        var currentShot = (Shot)_ShotPool.GetPoolable();
        //currentShot.SetShotDamage(_Model.Damage);
        //currentShot.SetShotVelocity(_Model.ShotVelocity);
        //currentShot.SetShotSize(_Model.ShotSizeX);
        currentShot.Fire(ShotSpawnPoint.position);
        //_Animator.SetTrigger("Shoot");
    }

    private void BuildShotPool()
    {
        _Shots = new List<IPoolable>();
        int count = _ShotPoolSize;
        while (count > 0)
        {
            count--;
            var clone = Instantiate(_ShotPrefab);

            var shot = clone.GetComponent<Shot>();
            _Shots.Add(shot);
            clone.transform.position = ShotSpawnPoint.position;
        }

        _ShotPool = new Pool(_Shots.ToArray());
    }

}
