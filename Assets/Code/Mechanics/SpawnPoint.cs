using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Transform _Target;
    [SerializeField]
    private GameObject _MobPrefabA, _MobPrefabB;
    [SerializeField]
    private int _RollTarget = 4, _Die = 9;

    private Pool _MobPoolA, _MobPoolB;
    private WTMK _Tools = WTMK.Instance;
    private Timer _Timer = new Timer();

    private void Awake()
    {
        _MobPoolA = BuildPool(_MobPrefabA, 25);
        _MobPoolB = BuildPool(_MobPrefabB, 25);        
    }

    private void Start()
    {
        //_Timer.OnTimerComplete += Spawn;
        //_Timer.Start(1000f);
    }

    private void Update()
    {
        //_Timer.Update();
    }

    public void Spawn()
    {
        Mob mob = null;
        var roll = _Tools.Rando.Next(_Die);
        if(roll > _RollTarget)
        {
            mob = (Mob)_MobPoolA.GetPoolable();
        }
        else
        {
            mob = (Mob)_MobPoolB.GetPoolable();
        }

        if(mob == null)
        {
            return;
        }

        mob.transform.position = transform.position;
        mob.transform.rotation = transform.rotation;
        mob.Spawn(_Target);
    }

    private Pool BuildPool(GameObject prefab, int size)
    {
        var pool = new List<IPoolable>();
        int count = size;
        while (count > 0)
        {
            count--;
            var clone = Instantiate(prefab);

            var shot = clone.GetComponent<Mob>();
            pool.Add(shot);
            clone.transform.position = transform.position;
            clone.SetActive(false);
        }

        return new Pool(pool.ToArray());
    }
}
