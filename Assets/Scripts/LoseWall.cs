using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseWall : MonoBehaviour
{
    public event System.Action enemyEnterned;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Enemy>(out _))
        {
            enemyEnterned?.Invoke();
        }
    }
}
