using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    [SerializeField]
    private Image _Value;

    public void SetValue(float value)
    {
        _Value.fillAmount = value;
    }
}
