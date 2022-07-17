using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Charger : Enemy
{
    [SerializeField] private int meleeDamage = 10;
    public override void Attack()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
            return;

        player.GetComponent<Entity>().TakeDamage(meleeDamage);
    }

    public override void Move()
    {
        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
            return;
        
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
