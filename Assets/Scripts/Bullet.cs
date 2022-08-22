using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private TrailRenderer _trail;

    private int _damage;
    private float _speed;

    private float _lifeTime = 4;
    private float _timeToLive;

    private TurretBulletsPool _pool;
    public int level { get; private set; }

    public void SetPool(TurretBulletsPool pool) => _pool = pool;

    private void Awake()
    {
        _timeToLive = _lifeTime;
    }

    public void Init(int level , int damage , Transform barrel)
    {
        this.level = level;

        _damage = damage;
        transform.position = barrel.position;
        transform.rotation = barrel.rotation;
    }

    private void FixedUpdate()
    {
        _timeToLive -= Time.deltaTime;
        if(_timeToLive <= 0)
        {
            ReturnToPool();
        }
    }

    public void Launch(float speed)
    {
        _speed = speed;

        _rigidBody.velocity = transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.health.Damage(_damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        _pool.ReturnBulletToPool(this);
    }

    public void ResetInstance()
    {
        _timeToLive = _lifeTime;
        _rigidBody.velocity = Vector3.zero;
        _trail.Clear();
        _damage = 0;
        _speed = 0;
    }
}
