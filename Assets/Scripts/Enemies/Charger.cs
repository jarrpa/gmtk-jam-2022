using System;
using UnityEngine;


public class Charger : Enemy
{
    [SerializeField] private int meleeDamage = 10;
    [SerializeField] private float chargeTime = 2f;
    [SerializeField] private float chargePower = 20f;
    [SerializeField] private float waitTime = 0.5f;

    private float _chargeTimer;
    private float _waitTimer;

    enum ChargerState
    {
        Chasing,
        Ready,
        Charging
    }

    private ChargerState _state = ChargerState.Chasing;

    private Vector2 _movement;
    public override void Attack()
    {
        rb.velocity = _movement * chargePower;
    }

    private void Start()
    {
        gameObject.GetComponent<Entity>().onHit.AddListener(OnHit);
    }

    public void Update()
    {
        Debug.Log(canMove);
        if(!canMove)
            return;
        
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
        
        //Vector2.Distance(path.vectorPath[currentWaypoint], player.position);
        
        if (Vector2.Distance(path.vectorPath[currentWaypoint], rb.position) < nextWaypointDistance)
            currentWaypoint++;

        switch (_state)
        {
            case ChargerState.Chasing:
                if (Vector2.Distance(player.position, rb.position) > stoppingDistance)
                {
                    rb.velocity = _movement * speed;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    _state = ChargerState.Ready;
                    _waitTimer = waitTime;
                }
                break;
            case ChargerState.Ready:
                if (_waitTimer > 0)
                {
                    _waitTimer -= Time.deltaTime;
                }
                else
                {
                    _state = ChargerState.Charging;
                    _chargeTimer = chargeTime;
                    Attack();
                }
                break;
            case ChargerState.Charging:
                if (_chargeTimer > 0) 
                    _chargeTimer -= Time.deltaTime;
                else
                    _state = ChargerState.Chasing;
                break;
                
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
            col.gameObject.GetComponent<Entity>().TakeDamage(meleeDamage);
    }

    public override void Move()
    { }
}