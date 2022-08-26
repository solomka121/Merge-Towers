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
    private bool _move = true;

    private ObjectPool<Enemy> _pool;
    private ParticlesPool _dieEffectsPool;
    private CoinsPool _coinsPool;

    private DifficultyManager _difficulty;

    public void SetPool(ObjectPool<Enemy> enemyPool, ParticlesPool dieEffectPool , CoinsPool coinsPool)
    {
        _pool = enemyPool;
        _dieEffectsPool = dieEffectPool;
        _coinsPool = coinsPool;
    }

    public void SetDifficulty(DifficultyManager difficulty)
    {
        _difficulty = difficulty;
    }

    private void Start()
    {
        health.OnDie += Death;
    }

    public void OnSpawn()
    {
        _coinsReward = _difficulty.enemiesCoinsDrop;

        _animator.SetBool("Run", true);
        health.SetmaxHealth(_difficulty.enemiesHealth);
        health.OnActivate();
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
        if (_move == false)
            return;

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

    public void Dance()
    {
        _move = false;
        _animator.SetBool("Run", false);
    }
}
