using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    [SerializeField] private Transform _enemiesParent;
    [SerializeField] private float _xSpawnRange;
    [SerializeField] private float _spawnTime;
    [field:SerializeField] public List<Enemy> activeEnemies { get; private set; }
    private float _timeToSpawn;

    private ObjectPool<Enemy> _enemyPool;
    private ObjectPool<ParticleSystem> _dieParticles;

    private int count = 0;

    void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnTakeEnemyFromPool, OnReturnEnemyToPool);
        _timeToSpawn = _spawnTime;
        activeEnemies = new List<Enemy>();
    }

    private void Update()
    {
        _timeToSpawn -= Time.deltaTime;
        if(_timeToSpawn <= 0 && count < 100)
        {
            count++;
            SpawnEnemy();
            _timeToSpawn = _spawnTime;
        }
    }

    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(_enemy, _enemiesParent);
        enemy.SetPool(_enemyPool);
        return enemy;
    }

    private void OnTakeEnemyFromPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(true);
        enemy.OnSpawn();

        activeEnemies.Add(enemy);
    }

    private void OnReturnEnemyToPool(Enemy enemy)
    {
        enemy.gameObject.SetActive(false);
        enemy.transform.localPosition = Vector3.zero;

        activeEnemies.Remove(enemy);
    }

    public void SpawnEnemy()
    {
        Enemy enemy = _enemyPool.Get();

        Vector3 spawnPosition = Vector3.zero;
        spawnPosition.x = Random.Range(-_xSpawnRange, _xSpawnRange);

        enemy.transform.localPosition += spawnPosition;
    }

    public Enemy GetClosestEnemy(Vector3 startPosition , int range)
    {
        if(activeEnemies.Count == 0)
            return null;

        Enemy closestEnemy = null;
        float closestDistanceSqr = range * range;

        for (int i = 0; i < activeEnemies.Count; i++)
        {
            Vector3 enemyPosition = activeEnemies[i].transform.position;
            Vector3 currentDirection = enemyPosition - startPosition;
            float currentDistance = currentDirection.sqrMagnitude;

            if (currentDistance < closestDistanceSqr)
            {
                closestEnemy = activeEnemies[i];
                closestDistanceSqr = currentDistance;
            }
        }

        return closestEnemy;

    }

}
