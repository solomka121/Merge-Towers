using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyTurret : MonoBehaviour
{
    [SerializeField] private PlayerWallet _wallet;
    [SerializeField] private BuildingManager _buildingManager;
    [SerializeField] private Building _buildingPrefab;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TMP_Text _costText;
    [SerializeField] private Color _cantAffordColor = Color.red;
    private Color _defaultColor;
    [SerializeField] private int _cost;

    private void Awake()
    {
        _buyButton.onClick.AddListener(Buy);
        _wallet.OnMoneyChange += CanAfford;
        _defaultColor = _costText.color;
        UpdateCostText();
    }

    public void Buy()
    {
        if (_buildingManager.IsThereEmptySpace() == false)
            return;

        if (_wallet.Buy(_cost))
        {
            _buildingManager.SpawnBuilding(_buildingPrefab);
            _cost++;

            UpdateCostText();
        }
    }

    private void UpdateCostText()
    {
        _costText.text = _cost.ToString();
    }

    private void CanAfford(int _)
    {
        Color color = _cantAffordColor;

        if (_wallet.CanAfford(_cost))
        {
            color = _defaultColor;
        }

        _costText.color = color;
    }
}
