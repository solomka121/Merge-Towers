using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWallet : MonoBehaviour
{
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private int _startMoney;
    private int moneyCount;

    private void Start()
    {
        moneyCount = _startMoney;
        UpdateText();
    }

    public void AddMoney(int count)
    {
        moneyCount += count;

        UpdateText();
        BumbMoneyText();
    }

    public void MinusMoney(int count)
    {
        moneyCount -= count;

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
        // bumb effect
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
