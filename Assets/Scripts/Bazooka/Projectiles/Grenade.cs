using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Bullet
{
    public float explosionRadius = 3f;
    public GameObject explosionEffect;
    public float timeUntilExplosion = 10f; // in seconds
    public float explosionForce = 10f;

    private bool hasExploded;

    protected override void Update()
    {
        if (timeUntilExplosion <= 0 || hasExploded)
            DestroyBullet();
        
        if (Vector2.Distance(startPosition, transform.position) >= range)
            hasExploded = true;
  
        timeUntilExplosion -= Time.deltaTime;

    }

    protected override void DestroyBullet()
    {
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
 
        foreach (Collider2D col in objectsInRange)
        {
            if (col.CompareTag("Player") || col.CompareTag("Enemy"))
            {
                Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                AddExplosionForce(rb, explosionForce, transform.position, explosionRadius);
            }
        }

        var particles = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        
        Destroy(particles, 5f);
        Destroy(gameObject);
    }
    
    public static void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var dir = (body.transform.position - explosionPosition);
        float wearoff = 1 - (dir.magnitude / explosionRadius);
        body.AddForce(dir.normalized * explosionForce * wearoff);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
