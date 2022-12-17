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

    public float CurrentValue => _CurrentTankValue;

    private float _CurrentTankValue;
    public void Deplete(float fuelCost)
    {
        if (_FuelBar.GetValue() <= 0)
        {
            OnEmpty?.Invoke();
            return;
        }

        _FuelBar.SetValue(_CurrentTankValue -= fuelCost);
    }

    public void Fill(float fuelCost)
    {
        if(_FuelBar.GetValue() >= 1)
        {
            return;
        }

        if (_CurrentTankValue <= 0)
        {
            OnHasFuel?.Invoke();
        }

        _FuelBar.SetValue(_CurrentTankValue += (fuelCost * .5f));
    }

    private void Start()
    {
        _CurrentTankValue = _BaseFuelValue;
        _FuelBar.SetValue(_CurrentTankValue);
    }
}
