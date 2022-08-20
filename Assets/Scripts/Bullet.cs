using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;

    private int _damage;
    private float _speed;


    public void Init(int damage)
    {
        _damage = damage;  
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
            Destroy(gameObject);
        }
    }
}
