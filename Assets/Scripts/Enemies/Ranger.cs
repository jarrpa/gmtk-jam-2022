using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranger : Enemy
{
    public GameObject bulletPrefab;
    private Vector2 _movement;

    private bool _canShoot = true;
    public override void Attack()
    {
        enemyAttackEvent?.Invoke(attack);
        InstantiateBullet();
    }
    
    private void InstantiateBullet()
    {
        var shootDir = ((Vector2) player.position - rb.position).normalized;
        var bullet = Instantiate(bulletPrefab, (Vector2)transform.position + shootDir * 0.8f, Quaternion.identity);
        bullet.GetComponent<Bullet>().Setup(shootDir, attack);
    }

    private void Update()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        
        reachedEndOfPath = false;

        _movement = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
        ChangeAnimationFromAngle(angle);

        if (Vector2.Distance(path.vectorPath[currentWaypoint], rb.position) < nextWaypointDistance)
            currentWaypoint++;
        
        Move();

    }

    public override void Move()
    {
        if (entity.isStunned) return;

        var playerDistance = Vector2.Distance(player.position, rb.position);
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.1f, player.transform.position - transform.position, playerDistance, LayerMask.GetMask("Obstacle"));

        if (playerDistance > stoppingDistance || hit.collider != null)
        {
            rb.velocity = _movement * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
            if (_canShoot)
                StartCoroutine(Shoot());
        }
       
    }

    private IEnumerator Shoot()
    {
        _canShoot = false;
        Attack();
        yield return new WaitForSeconds(attackRate);
        _canShoot = true;
    }
}
