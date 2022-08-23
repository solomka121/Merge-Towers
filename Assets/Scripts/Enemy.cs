using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _coinsReward;
    public HealthSystem health;
    public Vector3 direction = -Vector3.forward;

    private ObjectPool<Enemy> _pool;
    private ParticlesPool _dieEffectsPool;
    private CoinsPool _coinsPool;

    public void SetPool(ObjectPool<Enemy> enemyPool, ParticlesPool dieEffectPool , CoinsPool coinsPool)
    {
        _pool = enemyPool;
        _dieEffectsPool = dieEffectPool;
        _coinsPool = coinsPool;
    }

    private void Start()
    {
        health.Die += Death;
    }

    public void OnSpawn()
    {
        _animator.SetBool("Run", true);
    }
     
    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 effectsOffset = new Vector3(0, 0.5f, 0);
            _dieEffectsPool.ActivateParticle(transform.position + effectsOffset);
            _coinsPool.SpawnCoins(transform.position + effectsOffset, 0.5f, _coinsReward);
        }
    }

    private void FixedUpdate()
    {
        transform.position += direction * (_speed * Time.deltaTime);
    }

    private void Death()
    {
        Vector3 effectsOffset = new Vector3(0, 0.5f, 0);
        _dieEffectsPool.ActivateParticle(transform.position + effectsOffset);
        _coinsPool.SpawnCoins(transform.position + effectsOffset, 1f , _coinsReward);

        health.ResetHealth();
        _pool.Release(this);
    }
}
