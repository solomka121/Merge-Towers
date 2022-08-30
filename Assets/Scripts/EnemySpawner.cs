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

    private ObjectPool<Enemy> _enemyPool;
    [SerializeField] private ParticlesPool _dieParticlesPool;
    [SerializeField] private CoinsPool _coinsPool;

    public event System.Action OnStageClear;
    private int _enemiesToSpawn;
    [field:SerializeField] public List<Enemy> activeEnemies { get; private set; }
    private int _enemiesToClear;
    /*private float _timeToSpawn;*/
    private bool _canSpawn = true;

    private DifficultyManager _difficulty;

    void Awake()
    {
        _enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnTakeEnemyFromPool, OnReturnEnemyToPool);
        //_timeToSpawn = _spawnTime;
        activeEnemies = new List<Enemy>();
    }

    public void SetDifficulty(DifficultyManager difficulty)
    {
        _difficulty = difficulty;
    }

    private void Start()
    {
        _spawnTime = _difficulty.enemiesSpawnTime;
    }

/*    private void Update()
    {
        _timeToSpawn -= Time.deltaTime;
        if (_timeToSpawn <= 0)
        {
            if (_canSpawn == false)
                return;

            SpawnEnemy();
            _timeToSpawn = _spawnTime;
        }
    }*/

    public void SpawnEnemies(int count)
    {
        _spawnTime = _difficulty.enemiesSpawnTime;

        _enemiesToSpawn = count;
        _enemiesToClear = count;
        StartCoroutine(SpawningEnemies());
    }

    private IEnumerator SpawningEnemies()
    {
        for(int i = 0; _enemiesToSpawn > 0; _enemiesToSpawn--)
        {
            if (_canSpawn == false)
                yield return null;

            SpawnEnemy();
            yield return new WaitForSeconds(_spawnTime);
        }
    }

    public void CanSpawn(bool state)
    {
        _canSpawn = state;
    }

    private Enemy CreateEnemy()
    {
        Enemy enemy = Instantiate(_enemy, _enemiesParent);
        enemy.SetPool(_enemyPool , _dieParticlesPool , _coinsPool);
        enemy.SetDifficulty(_difficulty);
        enemy.health.OnDie += CountKill;
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

    public void SetEnemiesDance()
    {
        for (int i = 0; i < activeEnemies.Count; i++)
        {
            activeEnemies[i].Dance();
        }
    }

    public void CountKill()
    {
        _enemiesToClear--;
        if(_enemiesToClear == 0)
        {
            OnStageClear?.Invoke();
        }
    }
}
