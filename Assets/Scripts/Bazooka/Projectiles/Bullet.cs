using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletDamage;
    protected Vector2 startPosition;
    private Transform player;

    public float range;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Vector2.Distance(startPosition, transform.position) >= range)
            DestroyBullet();
    }

    protected virtual void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Took Damage");
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Took Damage");
            Destroy(gameObject);
        }
        DestroyBullet();
    }
}
