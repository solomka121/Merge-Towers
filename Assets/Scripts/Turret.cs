using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [field:SerializeField] public int level { get; private set; }
    [field: SerializeField] public int damage { get; private set; } = 1;
    [field: SerializeField] public float fireRate { get; private set; } = 1;

    private float _shootCooldown;
    private float _lastShootTimeSeconds;

    [SerializeField] private Transform _turretTopPart;
    [SerializeField] private Transform _barrelShootPoint;
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

    public void Update()
    {
        Aim();
        Shoot();
    }

    protected void Aim()
    {
        Enemy enemy = _enemySpawner.GetFirstEnemy();

        Vector3 lookDirection;
        if (enemy != null) 
        {
            lookDirection = enemy.transform.position - _turretTopPart.transform.position;
            lookDirection.y = _turretTopPart.transform.position.y;
        }
        else
        {
            lookDirection = Vector3.forward;
        }

        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        _turretTopPart.rotation = lookRotation;
    }

    protected void Shoot()
    {
        float timeElapsed = Time.time - _lastShootTimeSeconds;

        if(timeElapsed >= _shootCooldown)
        {
            
            _lastShootTimeSeconds = Time.time;
        }
    }
}
