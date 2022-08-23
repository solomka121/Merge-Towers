using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinValue;
    private PlayerWallet _wallet;
    private RectTransform _rect;
    private CoinsPool _pool;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    public void Init(CoinsPool pool , PlayerWallet wallet)
    {
        _pool = pool;
        _wallet = wallet;
    }

    public void Spawn(Vector2 centerPosition , Vector2 inRangePosition)
    {
        transform.localScale = Vector2.zero;
        _rect.position = centerPosition;
        LeanTween.scale(gameObject, Vector2.one, 0.6f).setEaseOutBack();
        LeanTween.move(gameObject, inRangePosition, 0.7f).setEaseOutCubic().setOnComplete(FlyToMoneyPanel);
    }

    public void FlyToMoneyPanel()
    {
        LeanTween.move(_rect, _wallet.moneyPanel.anchoredPosition , 0.8f).setEaseInBack();
        LeanTween.scale(gameObject , Vector2.zero , 0.9f).setEaseInBack().setOnComplete(AddMoney);
    }

    private void AddMoney()
    {
        _wallet.AddMoney(1);
        ResetInstance();
    }

    public void ResetInstance()
    {
        LeanTween.cancel(gameObject);
        _pool.ReturnCoinToPool(this);
    }
}
