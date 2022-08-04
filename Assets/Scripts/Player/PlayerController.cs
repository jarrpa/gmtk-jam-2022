using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 _mousePosition;
    private Vector2 _moveDir;
    private Rigidbody2D _rb;
    private AbilityHolder _abilityHolder;

    private bool _isAbilityActive;

    #region Animation Variables
    private string currentState;
    [SerializeField] private Animator animator;
    [SerializeField] private float rightAngleOffset;    // offset in degrees
    [SerializeField] private float leftAngleOffset;     // offset in degrees
    private const string PLAYER_IDLE_DOWN = "player_idle_down";
    private const string PLAYER_IDLE_LEFT = "player_idle_left";
    private const string PLAYER_IDLE_RIGHT = "player_idle_right";
    private const string PLAYER_IDLE_UP = "player_idle_up";
    private const string PLAYER_WALK_LEFT = "player_walk_left";
    private const string PLAYER_WALK_RIGHT = "player_walk_right";
    private const string PLAYER_WALK_UP = "player_walk_up";
    private const string PLAYER_WALK_DOWN = "player_walk_down";
    #endregion
    
    static PlayerController _instance;
    public static PlayerController instance => _instance;
    
    private void Awake()
    {
        _instance = this;
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0;
        
        _abilityHolder = GetComponent<AbilityHolder>();

        animator = GetComponent<Animator>();

        currentState = PLAYER_IDLE_DOWN;
    }
    
    void Update()
    {
        _moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        ChangeAnimationFromAngle(GetLookAngle());
    }

    private void FixedUpdate()
    {
       Move();
    }


    private void Move()
    {
        if (_abilityHolder.IsActive())
            return;
        if (_moveDir != Vector2.zero)
        {
            _rb.velocity = _moveDir * moveSpeed;
        }
        else
        {
            _rb.velocity = Vector2.zero;
           // ChangeAnimationState(PLAYER_IDLE_DOWN);
        }
    }

    public Vector2 GetVelocity()
    {
        return _moveDir;
    }

    public float GetLookAngle()
    {
        Vector3 lookDir = ((Vector3)_mousePosition - transform.position).normalized;
        return Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
    }

    private void ChangeAnimationFromAngle(float angle)
    {
        string newState = currentState;
        if ((angle >= 0 && angle <= rightAngleOffset) || (angle >= -rightAngleOffset && angle <= 0))
        {
            if(_rb.velocity != Vector2.zero)
                newState = PLAYER_WALK_RIGHT;
            else
                newState = PLAYER_IDLE_RIGHT;
        }
        else if ((angle >= leftAngleOffset && angle <= 180) || (angle >= -180 && angle <= -leftAngleOffset))
        {
            if(_rb.velocity != Vector2.zero)
                newState = PLAYER_WALK_LEFT;
            else
                newState = PLAYER_IDLE_LEFT;
        }
        else if (angle > rightAngleOffset && angle < leftAngleOffset)
        {
            if(_rb.velocity != Vector2.zero)
                newState = PLAYER_WALK_UP;
            else
                newState = PLAYER_IDLE_UP;
        }
        else if (angle > -leftAngleOffset && angle < -rightAngleOffset)
        {
            if(_rb.velocity != Vector2.zero)
                newState = PLAYER_WALK_DOWN;
            else
                newState = PLAYER_IDLE_DOWN;
        }
        ChangeAnimationState(newState);
    }

    public void ChangeAnimationState(string newState)
    {
        if (currentState.Equals(newState)) return;
        
        animator.Play(newState);

        currentState = newState;

    }

    public bool IsPressingMouseButton()
    {
        return Input.GetButton("Fire2");
    }
}
