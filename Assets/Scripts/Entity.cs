using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public enum EntityKind {
    Unknown,
    Player,
    Charger,
    Ranger,
}

public class Entity : MonoBehaviour
{
    public EntityKind kind = EntityKind.Unknown;
    [SerializeField] private int maxHealth = 100;
    public int currentHealth;

    public EntityEvent onHit;
    public EntityEvent onDeath;
    public EntityPayload entityPayload = new EntityPayload();
    [SerializeField] protected Animator animator;
    public bool isInvuln;
    public bool isStunned;

    void Awake()
    {
        currentHealth = maxHealth;
        entityPayload.entity = this;
        onHit ??= GameEventLoader.Load<EntityEvent>("EntityHitEvent");
        onDeath ??= GameEventLoader.Load<EntityEvent>("EntityDeathEvent");
    }

    public void TakeDamage(AttackPayload attack)
    {
        if (isInvuln || !this.isActiveAndEnabled) return;

        animator.Play("Blinking", animator.GetLayerIndex("Blinking"));
        currentHealth = Mathf.Clamp(currentHealth - attack.damage, 0, 100);
        entityPayload.damage = attack.damage;
        if (attack.isStun) StartCoroutine(Stunned(attack.stunDuration));
        onHit?.Invoke(entityPayload);
        if (currentHealth <= 0)
        {
            onDeath?.Invoke(entityPayload);
            if (!gameObject.CompareTag("Player"))
                Destroy(gameObject);
        }
    }

    IEnumerator Stunned(float duration) {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
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
