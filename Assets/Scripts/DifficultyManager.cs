using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private DifficultyModifiers _modifiers;
    public int enemiesSpawnCount { get; private set; }
    public float enemiesSpawnTime { get; private set; }
    public int enemiesHealth { get; private set; }
    public int enemiesCoinsDrop { get; private set; }

    public void Awake()
    {
        enemiesSpawnCount = _modifiers.startEnemiesCount;
        enemiesSpawnTime = _modifiers.startSpawnTime;
        enemiesHealth = _modifiers.startHealth;
        enemiesCoinsDrop = _modifiers.startCoinsDrop;
    }

    public void UpdateModifiers(int currentLevel)
    {
        if(currentLevel % _modifiers.enemiesCountIntencity == 0)
        {
            enemiesSpawnCount += _modifiers.enemiesCountAddModifier;
        }
        if (currentLevel % _modifiers.spawnTimeIntencity == 0)
        {
            enemiesSpawnTime *= _modifiers.spawnTimeModifier;
        }
        if (currentLevel % _modifiers.healthIntencity == 0)
        {
            enemiesHealth = Mathf.RoundToInt(enemiesHealth * _modifiers.healthModifier);
        }
        if (currentLevel % _modifiers.coinsModifierIntencity == 0)
        {
            enemiesCoinsDrop += _modifiers.coinsAddModifier;
        }
    }
}
