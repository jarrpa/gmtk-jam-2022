using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    
    public event Action OnDeath;
    public event Action OnHit;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (IsDead())
        {
            OnDeath?.Invoke();
            return;
        }
        
        currentHealth -= amount;
        OnHit?.Invoke();
    }

    public void SetHealth(int amount)
    {
        currentHealth = amount;
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
    
    
}
