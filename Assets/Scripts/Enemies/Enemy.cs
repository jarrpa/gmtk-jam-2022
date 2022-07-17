using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float stoppingDistance;
    [SerializeField] protected float attackRate;

    private bool canAttack = true;

    public Transform player;

    public abstract void Attack();
    public abstract void Move();

    IEnumerator StartAttacking()
    {
        canAttack = false;
        Attack();
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }

    private void Update()
    {
        if (canAttack)
            StartCoroutine(StartAttacking());
        
        Move();
    }
}