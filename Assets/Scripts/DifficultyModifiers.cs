using UnityEngine;

[CreateAssetMenu(menuName = "Difficulty/Modifiers")]
public class DifficultyModifiers : ScriptableObject
{
    [Header("Enemies Spawn Count")]
    public int startEnemiesCount = 10;
    public int enemiesCountAddModifier = 5;
    public int enemiesCountIntencity = 1;

    [Header("Enemies Spawn Rate")]
    public int startSpawnTime = 2;
    public float spawnTimeModifier = 0.8f;
    public int spawnTimeIntencity = 2;

    [Header("Enemies Health")]
    public int startHealth = 20;
    public float healthModifier = 1.2f;
    public int healthIntencity = 5;

    [Header("Enemies Loot")]
    public int startCoinsDrop = 1;
    public int coinsAddModifier = 1;
    public int coinsModifierIntencity = 3;

}
