using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterSpawnController : MonoBehaviour
{
    [SerializeField]
    private List<SpawnPoint> _SpawnPoint;
    [SerializeField]
    private List<float> _Speeds;
    [SerializeField]
    private bool _IsActive = true;

    private Timer _Timer = new Timer();
    private WTMK _Tools = WTMK.Instance;

    void Awake()
    {
        _Timer.OnTimerComplete += Spawn;
    }

    void Update()
    {
        if(_IsActive)
        {
            InitSpawning();
        }

        _Timer.Update();
    }

    private void InitSpawning()
    {
        if (!_Timer.IsTicking)
        {
            var roll = _Tools.Rando.Next(_Speeds.Count);
            var speed = _Speeds[roll];
            _Timer.Start(speed);
        }        
    }

    private void Spawn()
    {
        var roll = _Tools.Rando.Next(_SpawnPoint.Count);
        _SpawnPoint[roll].Spawn();
    }
}
