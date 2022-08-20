using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;

    private int _damage;
    private float _speed;

    private TurretBulletsPool _pool;

    public void SetPool(TurretBulletsPool pool) => _pool = pool;

    public void Init(int damage , Transform barrel)
    {
        _damage = damage;
        transform.position = barrel.position;
        transform.rotation = barrel.rotation;
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
            _pool.ReturnBulletToPool(this);
        }
    }

    public void ResetInstance()
    {
        _rigidBody.velocity = Vector3.zero;
        _damage = 0;
        _speed = 0;
    }
}
