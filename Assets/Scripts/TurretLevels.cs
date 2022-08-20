using UnityEngine;

[CreateAssetMenu(menuName = "Turrets/TurretLevels")]
public class TurretLevels : ScriptableObject
{
    public int maxLevel;
    public Building[] buildings;

    public Building GetTurretLevel(int level)
    {
        return buildings[level - 1];
    }
}
