using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    public HealthSystem health;
    public Vector3 direction = -Vector3.forward;

    private ObjectPool<Enemy> _pool;
    private ParticlesPool _dieEffectsPool;

    public void SetPool(ObjectPool<Enemy> enemyPool, ParticlesPool dieEffectPool)
    {
        _pool = enemyPool;
        _dieEffectsPool = dieEffectPool;
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
        if (Input.GetMouseButton(1))
        {
            Death();
        }
    }

    private void FixedUpdate()
    {
        transform.position += direction * (_speed * Time.deltaTime);
    }

    private void Death()
    {
        _dieEffectsPool.ActivateParticle(transform.position + new Vector3(0 , 0.5f , 0));

        health.ResetHealth();
        _pool.Release(this);
    }
}
