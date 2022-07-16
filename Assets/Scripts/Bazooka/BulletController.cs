using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletDamage;
    private Vector2 difference;
    private Vector2 startPosition;
    private Transform player;

    public float range;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter(Collision collision)
    {
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (Vector2.Distance(startPosition, transform.position) >= range)
            Destroy(gameObject);

        if (rb != null)
        {
            rb.velocity = difference * bulletSpeed * Time.fixedDeltaTime;
        }
    }

    private void OnEnable()
    {
        if (gameObject.CompareTag("PlayerProjectile"))
        {
            difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.position;
            difference.Normalize();
        }
        else if (gameObject.CompareTag("EnemyProjectile"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            difference = (Vector2)player.transform.position - rb.position;
            difference.Normalize();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Enemy Took Damage");
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player Took Damage");
            Destroy(gameObject);
        }
    }
}
