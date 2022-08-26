using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private int _maxHealth;
    [field:SerializeField] public int currentHealth { get; private set; }

    public event System.Action<int> OnHealthChange;
    public event System.Action OnDie;

    private void Start()
    {
        currentHealth = _maxHealth;
        OnHealthChange += _healthBar.UpdateValue;
    }

    public void OnActivate()
    {
        _healthBar.Init(_maxHealth);
        ResetHealth();
    }

    public void SetmaxHealth(int health)
    {
        _maxHealth = health;
    }

    public void Damage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDie?.Invoke();
        }

        OnHealthChange?.Invoke(currentHealth);
    }

    public void ResetHealth() 
    {
        currentHealth = _maxHealth;
        OnHealthChange?.Invoke(currentHealth);
    }
}
