using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private Transform _Target;
    [SerializeField]
    private GameObject _MobPrefabA, _MobPrefabB;

    private Pool _MobPoolA, _MobPoolB;
    private WTMK _Tools = WTMK.Instance;

    private void Awake()
    {
        _MobPoolA = BuildPool(_MobPrefabA, 25);
        _MobPoolB = BuildPool(_MobPrefabB, 25);
    }

    private void Spawn()
    {
        Mob mob = null;
        var roll = _Tools.Rando.Next(9);
        if(roll > 4)
        {
            mob = (Mob)_MobPoolA.GetPoolable();
        }
        else
        {
            mob = (Mob)_MobPoolB.GetPoolable();
        }

        mob.transform.position = transform.position;
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

            var shot = clone.GetComponent<Shot>();
            pool.Add(shot);
            clone.transform.position = transform.position;
            clone.SetActive(false);
        }

        return new Pool(pool.ToArray());
    }
}
