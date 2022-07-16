using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionManager : MonoBehaviour
{
    private Health health;
    private DamageAmount hitDamageAmount;
    private float hitDamageValue;
    private void Start()
    {
        health = GetComponent<Health>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            hitDamageAmount = collision.gameObject.GetComponent<DamageAmount>();
            hitDamageValue = hitDamageAmount.GetDamageAmount();
            health.TakeDamage(hitDamageValue);
            Destroy(collision.gameObject);
            Debug.Log("Enemy took " + hitDamageValue + " amount of damage");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            hitDamageAmount = collision.gameObject.GetComponent<DamageAmount>();
            hitDamageValue = hitDamageAmount.GetDamageAmount();
            health.TakeDamage(hitDamageValue);
            Destroy(collision.gameObject);
            Debug.Log("Enemy took " + hitDamageValue + " amount of damage");
        }
    }
}
