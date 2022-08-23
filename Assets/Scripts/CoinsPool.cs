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
        for(int i = 0; i < count; i++)
        {
            SpawnCoin(position, range);
        }
    }

    public void SpawnCoin(Vector3 position , float range)
    {
        Coin coin = GetCoinFromQueue();
        coin.gameObject.SetActive(true);
        
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
