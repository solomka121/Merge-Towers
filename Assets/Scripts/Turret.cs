using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [field:SerializeField] public int level { get; private set; }
    [field: SerializeField] public int damage { get; private set; } = 1;
    [field: SerializeField] public float bulletSpeed { get; private set; } = 1;
    [field: SerializeField] public float fireRate { get; private set; } = 1;
    [field: SerializeField] public int Range { get; private set; } = 15;

    private float _shootCooldown;
    private float _lastShootTimeSeconds;

    [SerializeField] private Transform _turretTopPart;
    [SerializeField] private Transform _barrelShootPoint;
    [SerializeField] private Bullet _bullet;
    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        _shootCooldown = 1 / fireRate;
        damage *= level;
        fireRate *= level;
    }

    public void SetSpawner(EnemySpawner spawner)
    {
        _enemySpawner = spawner;
    }

    public void FixedUpdate()
    {
        Enemy enemy = _enemySpawner.GetClosestEnemy(transform.position, Range);

        Aim(enemy);

        if (enemy == null)
        {
            _lastShootTimeSeconds = Time.time + Random.Range(0 , 0.3f);
            return;
        }

        Shoot();
    }

    protected void Aim(Enemy enemy)
    {
        Vector3 lookDirection;
        if (enemy != null) 
        {
            lookDirection = enemy.transform.position - _turretTopPart.position;
            lookDirection.y = 0;
        }
        else
        {
            lookDirection = Vector3.forward;
        }

        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        Quaternion smoothedLookRotation = Quaternion.Slerp(_turretTopPart.rotation, lookRotation, 0.2f);

        _turretTopPart.rotation = smoothedLookRotation;


    }

    protected void Shoot()
    {
        float timeElapsed = Time.time - _lastShootTimeSeconds;

        if(timeElapsed >= _shootCooldown)
        {
            Bullet bullet = Instantiate(_bullet, _barrelShootPoint.position , _barrelShootPoint.rotation);
            bullet.Init(damage);
            bullet.Launch(bulletSpeed);
            _lastShootTimeSeconds = Time.time;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
