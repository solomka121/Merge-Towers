using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TurretBulletsPool : MonoBehaviour
{
    [SerializeField] private Transform bulletsParent;
    private Transform[] bulletsLevelParent;
    [SerializeField] private TurretBullets _turretBullets;
    private Dictionary<int, Queue<Bullet>> _bulletsPool;

    private void Awake()
    {
        _bulletsPool = new Dictionary<int, Queue<Bullet>>();
        bulletsLevelParent = new Transform[_turretBullets.bullets.Length];

        for (int i = 0; i < _turretBullets.bullets.Length; i++)
        {
            _bulletsPool.Add(i, new Queue<Bullet>());

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
        _bulletsPool[levelIndex].Enqueue(currentBullet);
        currentBullet.SetPool(this);

        currentBullet.transform.parent = bulletsLevelParent[levelIndex];

        return currentBullet;
    }

    public Bullet GetBulletFromPool(int level)
    {
        Bullet bullet;

        if (_bulletsPool[level - 1].TryPeek(out _) == false)
        {
            CreateBullet(level);
        }

        bullet = _bulletsPool[level - 1].Dequeue();

        return bullet;
    }

    public void ReturnBulletToPool(Bullet bullet)
    {
        bullet.ResetInstance();
        bullet.gameObject.SetActive(false);
        bullet.transform.localPosition = Vector3.zero;

        _bulletsPool[bullet.level - 1].Enqueue(bullet);
    }

}
