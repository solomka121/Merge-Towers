using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(menuName = "Turrets/TurretBullets")]
public class TurretBullets : ScriptableObject
{
    public Bullet[] bullets;
}
