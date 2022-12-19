using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelTank : MonoBehaviour
{
    public event Action OnEmpty;
    public event Action OnHasFuel;

    [SerializeField]
    private FuelBar _FuelBar;
    [SerializeField]
    private float _BaseFuelValue = 1, _BoostMinRange = 0.1f, _BoostMaxRange = 0.185f;

    public float CurrentValue => _FuelBar.GetValue();

    public void Deplete(float fuelCost)
    {
        var fuel = _FuelBar.GetValue();
        if (fuel <= 0)
        {
            OnEmpty?.Invoke();
            return;
        }

        _FuelBar.SetValue(fuel -= fuelCost);
    }

    public void Fill(float fuelCost)
    {
        var fuel = _FuelBar.GetValue();
        if (fuel >= 1)
        {
            return;
        }

        if (fuel <= 0)
        {
            OnHasFuel?.Invoke();
        }

        _FuelBar.SetValue(fuel += (fuelCost * .5f));
    }

    private void Start()
    {
        _FuelBar.SetValue(1f);
    }
}
