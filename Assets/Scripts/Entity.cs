using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;
    public UnityEvent onDeath;
    public UnityEvent<int> onHit;
    public float nextWaypointDistance = 3f;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, 100);
        onHit?.Invoke(currentHealth);
        if (currentHealth <= 0)
        {
            onDeath?.Invoke();
            if(gameObject.CompareTag("Player"))
                return;
            
            //onDeath?.RemoveAllListeners();
            Destroy(gameObject);
        }
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
