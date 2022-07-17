using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float stoppingDistance;
    [SerializeField] protected float attackRate;
    [SerializeField] protected float preAttackAudioDuration = 0f; // this should be the length of pre attack audio

    protected Rigidbody2D rb;
    private bool canAttack = true;

    public event Action OnAttack;

    public Transform player;

    public abstract void Attack();
    public abstract void Move();

    IEnumerator StartAttacking()
    {
        canAttack = false;
        OnAttack?.Invoke();
        yield return new WaitForSeconds(preAttackAudioDuration);
        Attack();
        yield return new WaitForSeconds(attackRate);
        canAttack = true;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    
    private void Update()
    {
        if (canAttack)
            StartCoroutine(StartAttacking());
        
        Move();
    }
}