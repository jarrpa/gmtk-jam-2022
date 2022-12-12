using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public static string kind = "Enemy";
    [SerializeField] protected float speed;
    [SerializeField] protected float stoppingDistance;
    [SerializeField] protected float attackRate;

    protected Rigidbody2D rb;

    public float nextWaypointDistance = 3f;
    protected Path path;
    protected int currentWaypoint = 0;
    protected bool reachedEndOfPath = false;

    public Seeker seeker;
    
    private float _dirAngle;
    protected AIPath AIPath;

    #region Animation Variables
    private string _currentState = ENEMY_IDLE_DOWN;
    [SerializeField] protected Animator animator;
    [SerializeField] protected float rightAngleOffset;    // offset in degrees
    [SerializeField] protected float leftAngleOffset;     // offset in degrees
    private const string ENEMY_IDLE_DOWN = "enemy_idle_down";
    private const string ENEMY_IDLE_LEFT = "enemy_idle_left";
    private const string ENEMY_IDLE_RIGHT = "enemy_idle_right";
    private const string ENEMY_IDLE_UP = "enemy_idle_up";
    private const string ENEMY_WALK_LEFT = "enemy_walk_left";
    private const string ENEMY_WALK_RIGHT = "enemy_walk_right";
    private const string ENEMY_WALK_UP = "enemy_walk_up";
    private const string ENEMY_WALK_DOWN = "enemy_walk_down";
    #endregion

    public Transform player;
    protected AttackEvent enemyAttackEvent;
    protected AttackPayload attack =  new AttackPayload();
    protected Entity entity;

    public abstract void Attack();
    public abstract void Move();

    protected void ChangeAnimationFromAngle(float angle)
    {
        string newState = _currentState;
        if ((angle >= 0 && angle <= rightAngleOffset) || (angle >= -rightAngleOffset && angle <= 0))
        {
            if(rb.velocity != Vector2.zero)
                newState = ENEMY_WALK_RIGHT;
            else
                newState = ENEMY_IDLE_RIGHT;
        }
        else if ((angle >= leftAngleOffset && angle <= 180) || (angle >= -180 && angle <= -leftAngleOffset))
        {
            if(rb.velocity != Vector2.zero)
                newState = ENEMY_WALK_LEFT;
            else
                newState = ENEMY_IDLE_LEFT;
        }
        else if (angle > rightAngleOffset && angle < leftAngleOffset)
        {
            if(rb.velocity != Vector2.zero)
                newState = ENEMY_WALK_UP;
            else
                newState = ENEMY_IDLE_UP;
        }
        else if (angle > -leftAngleOffset && angle < -rightAngleOffset)
        {
            if(rb.velocity != Vector2.zero)
                newState = ENEMY_WALK_DOWN;
            else
                newState = ENEMY_IDLE_DOWN;
        }
        ChangeAnimationState(newState);
    }

    protected void ChangeAnimationState(string newState)
    {
        if (_currentState.Equals(newState)) return;
        
        animator.Play(newState);

        _currentState = newState;

    }
    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker.GetComponent<Seeker>();
        entity = GetComponent<Entity>();
        attack.attacker = entity;
        enemyAttackEvent ??= GameEventLoader.Load<AttackEvent>("EnemyAttackEvent");

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    
        player = PlayerController.instance.gameObject.transform;
    
    }

    private void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, player.position, OnPathComplete);
    }

    protected void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
}