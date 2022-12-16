using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelTank : MonoBehaviour
{
    [SerializeField]
    private FuelBar _FuelBar;
    [SerializeField]
    private float _BaseFuelValue = 1, _BoostMinRange = 0.1f, _BoostMaxRange = 0.185f;

    public float CurrentValue => _CurrentTankValue;

    private float _CurrentTankValue;
    public void Deplete(float fuelCost)
    {
        _FuelBar.SetValue(_CurrentTankValue -= fuelCost);
    }

    private void Start()
    {
        _FuelBar.SetValue(_BaseFuelValue);
    }
}
