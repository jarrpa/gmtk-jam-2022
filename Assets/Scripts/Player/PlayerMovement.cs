using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    private Vector2 mousePosition;
    private Vector2 velocity;

    private Rigidbody2D rb;
    private AbilityHolder abilityHolder;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        abilityHolder = GetComponent<AbilityHolder>();
    }
    
    void Update()
    {
        velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        animator.SetBool("isMoving", true);
        animator.SetFloat("moveX", velocity.x);
        animator.SetFloat("moveY", velocity.y);
        Move();
        
    }

    private void Move()
    {
        animator.SetBool("isMoving", true);
        if(abilityHolder.IsActive())
           return; 
        Vector2 lookDirection = GetLookDirection();
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        rb.velocity = velocity * moveSpeed;
        //rb.MovePosition(rb.position + velocity * moveSpeed * Time.fixedDeltaTime);
    }

    public Vector2 GetVelocity()
    {
        return velocity;
    }

    public Vector2 GetLookDirection()
    {
        return mousePosition - rb.position;
    }
}
