using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int bulletDamage;
    [SerializeField] private bool isStun;
    [SerializeField] private float stunDuration;
    protected Vector2 startPosition;
    protected AttackPayload attack;
    private Vector2 shootDirection;

    public float range;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    public void Setup(Vector2 direction, AttackPayload attackPayload)
    {
        shootDirection = direction;
        attack = attackPayload;
        attack.damage = bulletDamage;
        attack.isStun = isStun;
        attack.stunDuration = stunDuration;
        rb.AddForce(shootDirection * bulletSpeed);
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
        if (collision.gameObject == null)
            return;
        var entity = collision.gameObject.GetComponent<Entity>();
        if (entity != null && !collision.gameObject.CompareTag(attack.attacker.tag))
        {
            entity.TakeDamage(attack);
        }
        DestroyBullet();
    }
}
