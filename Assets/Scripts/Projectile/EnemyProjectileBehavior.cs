using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBehavior : ProjectileBehavior
{
    private Transform player;
    
    protected override void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        difference = (Vector2)player.transform.position - rb.position;
        difference.Normalize();
        StartCoroutine(DestroyTimer(timeTillDestroy));
    }
}
