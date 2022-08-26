using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsPool : MonoBehaviour
{
    [SerializeField] private Coin _prefab;
    [SerializeField] private Transform _canvasCoinsParent;
    [SerializeField] private PlayerWallet _wallet;
    private Camera _mainCamera;
    private Queue<Coin> _coins;

    private void Awake()
    {
        _coins = new Queue<Coin>();
        _mainCamera = Camera.main;
    }

    public void SpawnCoins(Vector3 position, float range , int count)
    {
        StartCoroutine(CoinsQueue(position, range, count));
    }

    private IEnumerator CoinsQueue(Vector3 position, float range , int count)
    {
        List<int> valuesList = RandomCoinsValue(count);

        for (int i = 0; i < valuesList.Count; i++)
        {
            SpawnCoin(position, range , valuesList[i]);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private List<int> RandomCoinsValue(int totalValue)
    {
        List<int> result = new List<int>();
        int valueLeft = totalValue;

        if (totalValue <= 1)
        {
            result.Add(1);
            return result;
        }

        while (valueLeft > 0)
        {
            int currentValue = Random.Range(1, valueLeft + 1);
            valueLeft -= currentValue;
            result.Add(currentValue);
        }

        return result;
    }

    public void SpawnCoin(Vector3 position , float range , int value)
    {
        Coin coin = GetCoinFromQueue();
        coin.gameObject.SetActive(true);
        coin.SetValue(value);
        
        Vector3 centerPosition = _mainCamera.WorldToScreenPoint(position);
        Vector3 randomOffset = new Vector3(Random.Range(-range, range), Random.Range(-range, 0), Random.Range(-range, range));
        randomOffset.y -= range;
        Vector3 inRangePosition = _mainCamera.WorldToScreenPoint(position + randomOffset);
        
        coin.Spawn(centerPosition, inRangePosition);
    }

    private void CreateCoin()
    {
        Coin coin = Instantiate(_prefab, _canvasCoinsParent);
        coin.Init(this , _wallet);
        _coins.Enqueue(coin); 
    }

    public Coin GetCoinFromQueue()
    {
        Coin coin;
        if(_coins.TryPeek(out _) == false)
        {
            CreateCoin();
        }

        coin = _coins.Dequeue();

        return coin;
    }

    public void ReturnCoinToPool(Coin coin)
    {
        coin.gameObject.SetActive(false);
        _coins.Enqueue(coin);
    }
    
}
