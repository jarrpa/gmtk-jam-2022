using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Smash")]
public class SmashAbility : Ability
{
    public float knockback = 20f;
    public float radius = 2f;
    public int damage = 5;
    
    
    public override void Activate(GameObject parent)
    {
        SmashAction(parent);
    }

    public override void Deactivate()
    {

    }

    void SmashAction(GameObject parent)
    {
        parent.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(parent.transform.position, radius);
 
        foreach (Collider2D col in objectsInRange)
        {
            if (col.CompareTag("Enemy"))
            {
                var dir = (col.transform.position - parent.transform.position);
                float wearoff = 1 - (dir.magnitude / radius);
                col.GetComponent<Rigidbody2D>().velocity = dir.normalized * knockback * wearoff;
                
                col.GetComponent<Entity>().TakeDamage(damage);
            }
        }
    }
}
