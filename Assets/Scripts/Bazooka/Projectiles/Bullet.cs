using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int bulletDamage;
    protected Vector2 startPosition;
    private Transform player;
    private Vector2 shootDirection;

    public float range;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Vector2 direction)
    {
        shootDirection = direction;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Vector2.Distance(startPosition, transform.position) >= range)
            DestroyBullet();

        transform.position = (Vector2) transform.position + shootDirection * bulletSpeed * Time.deltaTime;
    }

    protected virtual void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Entity>().TakeDamage(bulletDamage);
            
        }
        DestroyBullet();
    }
}
