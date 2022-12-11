using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillAreaWithPrefab : MonoBehaviour
{
    [SerializeField]
    private GameObject _Prefab;
    [SerializeField]
    private int XSize, YSize;
    [SerializeField]
    private float XSpace, YSpace;
    [SerializeField]
    private float XStart, YStart;


    private List<GameObject> _Spawned;
    private void Start()
    {
        _Spawned = new List<GameObject>();
        for (int i = 0; i < XSize; i++)
        {
            for (int j = 0; j < YSize; j++)
            {
                var clone = Instantiate(_Prefab);
                clone.transform.SetParent(transform);
                clone.transform.position = new Vector3(XStart + XSpace,0f, YStart + YSpace);
                _Spawned.Add(clone);
            }
        }
    }

}
