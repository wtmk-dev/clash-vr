using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{    
    [SerializeField]
    private List<BoostRing> _BoostRings;
    [SerializeField]
    private FlyingMechanics _FlyingMechanics;

    private void Awake()
    {
        for (int i = 0; i < _BoostRings.Count; i++)
        {
            _BoostRings[i].OnTriggered += BoostRingTriggered;
        }
    }

    private void BoostRingTriggered()
    {
        _FlyingMechanics.SetBoostable(true);
    }
}
