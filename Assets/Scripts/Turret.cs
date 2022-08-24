using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [field:SerializeField] public int level { get; private set; }
    [field: SerializeField] public int damage { get; private set; } = 1;
    [field: SerializeField] public float bulletSpeed { get; private set; } = 1;
    [field: SerializeField] public float fireRate { get; private set; } = 1;
    [field: SerializeField] public int range { get; private set; } = 15;

    [field: SerializeField] public Color mainColor { get; private set; }

    private float _shootCooldown;
    private float _lastShootTimeSeconds;

    [SerializeField] private Transform _turretTopPart;
    [SerializeField] private TurretBarrel[] _barrel;
    private int _previousShootBarrel;
    [SerializeField] private TurretBulletsPool _bulletsPool;
    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        _shootCooldown = 1 / fireRate;
        damage *= level;
        fireRate *= level;
    }

    public void SetPool(TurretBulletsPool pool) => _bulletsPool = pool;

    public void SetSpawner(EnemySpawner spawner) => _enemySpawner = spawner;

    public void FixedUpdate()
    {
        Enemy enemy = _enemySpawner.GetClosestEnemy(transform.position, range);

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
        Quaternion smoothedLookRotation = Quaternion.Slerp(_turretTopPart.rotation, lookRotation, 0.14f);

        _turretTopPart.rotation = smoothedLookRotation;


    }

    protected void Shoot()
    {
        float timeElapsed = Time.time - _lastShootTimeSeconds;

        if(timeElapsed >= _shootCooldown)
        {
            ChangeShootBarrel();
            TurretBarrel barrel = _barrel[_previousShootBarrel];

            Bullet bullet = _bulletsPool.GetBullet(level);
            bullet.Init(level , damage , barrel.shootPoint);
            bullet.Launch(bulletSpeed);
            _lastShootTimeSeconds = Time.time;

            barrel.Recoil();
        }
    }

    private void ChangeShootBarrel()
    {
        if(_previousShootBarrel == _barrel.Length - 1)
        {
            _previousShootBarrel = 0;
            return;
        }

        _previousShootBarrel++;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
