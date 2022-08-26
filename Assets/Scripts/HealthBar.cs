using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private int _maxValue;
    [SerializeField] private int _currentValue;
    [SerializeField] public bool _dontShowIfFull = true;

    public void Init(int max)
    {
        _maxValue = max;
        _currentValue = _maxValue;

        if (_dontShowIfFull)
        {
            SetActive(false);
        }
    }

    public void UpdateValue(int value)
    {
        _currentValue = value;
        UpdateBar();
        SetActive(true);
    }

    public void UpdateBar()
    {
        _fillImage.fillAmount = (float)_currentValue / _maxValue;
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }
}
