using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    [SerializeField] protected float classBulletSpeed;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected float timeTillDestroy;
    protected Vector2 difference;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rb != null)
        {
            rb.velocity = difference * classBulletSpeed * Time.fixedDeltaTime;
        }
    }

    virtual protected void OnEnable()
    {
        difference = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.position;
        difference.Normalize();
        StartCoroutine(DestroyTimer(timeTillDestroy));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    protected IEnumerator DestroyTimer(float destroyTime)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
