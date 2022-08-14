using UnityEngine;

[CreateAssetMenu(menuName = "Turrets/TurretsLevel")]
public class TurretsLevels : ScriptableObject
{
    public int maxLevel;
    public Building[] buildings;

    public Building GetTurretLevel(int level)
    {
        return buildings[level - 1];
    }
}
