using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Triggerable
{
    public Transform ShotSpawnPoint;
    [SerializeField]
    private GameObject _ShotPrefab, _ShotBPrefab;
    [SerializeField]
    private int _ShotPoolSize;
    [SerializeField]
    private Vector3 _ShotDirection;

    public override void Trigger(int trigger = 0)
    {
        base.Trigger(trigger);
        Fire(trigger);
    }

    private Pool _ShotPool, _ShotPoolB;
    private List<IPoolable> _Shots, _ShotsB;

    private void Awake()
    {
        _ShotPool = BuildShotPool(_Shots, _ShotPrefab);
        _ShotPoolB = BuildShotPool(_ShotsB, _ShotBPrefab); 
    }

    private void Fire(int trigger)
    {
        Shot currentShot = null;

        if(trigger == 0)
        {
            currentShot = (Shot)_ShotPool.GetPoolable();
        }else
        {
            currentShot = (Shot)_ShotPoolB.GetPoolable();
        }
       
        currentShot.transform.position = ShotSpawnPoint.position;
        currentShot.Fire(ShotSpawnPoint.forward);
    }

    private Pool BuildShotPool(List<IPoolable> pool, GameObject prefab)
    {
        pool = new List<IPoolable>();
        int count = _ShotPoolSize;
        while (count > 0)
        {
            count--;
            var clone = Instantiate(prefab);

            var shot = clone.GetComponent<Shot>();
            pool.Add(shot);
            clone.transform.position = ShotSpawnPoint.position;
        }

       return new Pool(pool.ToArray());
    }

}
