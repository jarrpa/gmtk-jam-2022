using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Bullet
{
    public float explosionRadius = 3f;
    public GameObject explosionEffect;
    public float explosionForce = 10f;
    public GameEvent explosionEvent;

    private void Awake()
    {
        explosionEvent ??= GameEventLoader.Load<GameEvent>("GrenadeExplosionEvent");
    }

    private void Explode()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D col in objectsInRange)
        {
            var entity = col.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                AddExplosionForce(rb, explosionForce, transform.position, explosionRadius);

                entity.TakeDamage(attack);
            }
        }

        var particles = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        explosionEvent?.Invoke();
        Destroy(particles, 5f);
    }

    protected override void DestroyBullet()
    {
        Explode();
        Destroy(gameObject);
    }

    public static void AddExplosionForce(Rigidbody2D body, float force, Vector3 explosionPosition, float explosionRadius)
    {
        Vector2 dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir * force);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
