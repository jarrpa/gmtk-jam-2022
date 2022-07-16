using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float stoppingDistance;
    [SerializeField] protected float retreatDistance;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float startTimeBetweenShots;
    private float timeBetweenShots;
    public Transform player;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = startTimeBetweenShots;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.position) < stoppingDistance
            && Vector2.Distance(transform.position, player.position) > retreatDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

        if (timeBetweenShots <= 0)
        {
            Instantiate(projectile, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }
}
