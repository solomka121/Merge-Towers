using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private StageCompletedPanel _nextLevelPanel;
    [SerializeField] private EnemySpawner _enemies;
    [SerializeField] private DifficultyManager _difficulty;
    [SerializeField] private BuildingManager _buildings;
    [SerializeField] private LoseWall _loseWall;

    [field:SerializeField] public int level { get; private set; }

    private void Awake()
    {
        _loseWall.onEnemyEnterned += Lose;
        _enemies.SetDifficulty(_difficulty);
        _enemies.OnStageClear += NextLevel;
        _nextLevelPanel.OnContinueButtonClick += StartLevel;
    }

    private void Start()
    {
        _enemies.SpawnEnemies(_difficulty.enemiesSpawnCount);
    }

    public void NextLevel()
    {
        level++;
        _difficulty.UpdateModifiers(level);
        ShowWinPanel();
    }

    public void ShowWinPanel()
    {
        _nextLevelPanel.ShowPanel(level);
    }

    public void StartLevel()
    {
        _nextLevelPanel.HidePanel();
        _enemies.SpawnEnemies(_difficulty.enemiesSpawnCount);
    }

    private void Lose()
    {
        _enemies.CanSpawn(false);
        _enemies.SetEnemiesDance();
        _buildings.DiactivateTurrets();
        Debug.Log("lose");
    }
}
