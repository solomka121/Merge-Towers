using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [field:SerializeField] public int currentHealth { get; private set; }

    public event System.Action OnHealthChange;
    public event System.Action Die;

    private void Start()
    {
        currentHealth = _maxHealth;
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die?.Invoke();
        }

        OnHealthChange?.Invoke();
    }

    public void ResetHealth() 
    {
        currentHealth = _maxHealth;
        OnHealthChange?.Invoke();
    }
}
