using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TurretBulletsPool : MonoBehaviour
{
    [SerializeField] private Transform bulletsParent;
    private Transform[] bulletsLevelParent;
    [SerializeField] private TurretBullets _turretBullets;
    private Dictionary<int, Queue<Bullet>> _bullets;

    private void Awake()
    {
        _bullets = new Dictionary<int, Queue<Bullet>>();
        bulletsLevelParent = new Transform[_turretBullets.bullets.Length];

        for (int i = 0; i < _turretBullets.bullets.Length; i++)
        {
            _bullets.Add(i, new Queue<Bullet>());

            GameObject levelParent = new GameObject(($"Level {i + 1}"));
            levelParent.transform.parent = bulletsParent;
            bulletsLevelParent[i] = levelParent.transform;
        }
    }

    public Bullet GetBullet(int level)
    {
        Bullet bullet = GetBulletFromPool(level);

        bullet.gameObject.SetActive(true);
        return bullet;
    }

    private Bullet CreateBullet(int level)
    {
        int levelIndex = level - 1;

        Bullet currentBullet = Instantiate(_turretBullets.bullets[levelIndex]);
        _bullets[levelIndex].Enqueue(currentBullet);
        currentBullet.SetPool(this);

        currentBullet.transform.parent = bulletsLevelParent[levelIndex];

        return currentBullet;
    }

    public Bullet GetBulletFromPool(int level)
    {
        Bullet bullet;

        if (_bullets[level - 1].TryDequeue(out bullet))
        {
            return bullet;
        }

        CreateBullet(level);
        bullet = _bullets[level - 1].Dequeue();

        return bullet;
    }

    public void ReturnBulletToPool(Bullet bullet)
    {
        bullet.ResetInstance();
        bullet.gameObject.SetActive(false);
        bullet.transform.localPosition = Vector3.zero;

        _bullets[bullet.level - 1].Enqueue(bullet);
    }

}
