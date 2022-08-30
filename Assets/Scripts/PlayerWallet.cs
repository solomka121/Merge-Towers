using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;
    [field:SerializeField] public RectTransform moneyPanel { get; private set; }
    [SerializeField] private int _startMoney;
    public int moneyCount { get; private set; }
    public event System.Action<int> OnMoneyChange;

    private void Awake()
    {
        moneyCount = _startMoney;
        UpdateText();
    }

    public void AddMoney(int count)
    {
        moneyCount += count;

        OnMoneyChange?.Invoke(moneyCount);

        UpdateText();
        BumbMoneyText();
    }

    public void MinusMoney(int count)
    {
        moneyCount -= count;

        OnMoneyChange?.Invoke(moneyCount);

        UpdateText();
        BumbMoneyText();
    }

    public bool Buy(int value)
    {
        if (CanAfford(value))
        {
            MinusMoney(value);
            return true;
        }
        return false;
    }

    public bool CanAfford(int value)
    {
        return moneyCount >= value;
    }

    private void BumbMoneyText()
    {
        _moneyText.transform.localScale = Vector3.one;
        LeanTween.scale(_moneyText.gameObject, new Vector3(1.2f, 1.2f, 1.2f), 0.1f).setEaseOutCubic().setLoopPingPong(1);
    }

    private void UpdateText()
    {
        if(moneyCount >= 1000)
        {
            _moneyText.text = (moneyCount / 1000f).ToString("F1") + "K";
        }
        else
        {
            _moneyText.text = moneyCount.ToString();
        }
    }
}
