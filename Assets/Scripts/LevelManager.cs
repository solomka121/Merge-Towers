using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemies;
    [SerializeField] private DifficultyManager _difficulty;
    [SerializeField] private BuildingManager _buildings;
    [SerializeField] private LoseWall _loseWall;

    private void Awake()
    {
        _loseWall.onEnemyEnterned += Lose;
        _enemies.SetDifficulty(_difficulty);
    }

    private void Lose()
    {
        _enemies.CanSpawn(false);
        _enemies.SetEnemiesDance();
        _buildings.DiactivateTurrets();
        Debug.Log("lose");
    }
}
