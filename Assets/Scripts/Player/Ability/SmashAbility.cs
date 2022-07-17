using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Smash")]
public class SmashAbility : Ability
{
    public float knockback;
    public float radius = 2f;
    public float jumpingAnimationTime = 2f;
    public int damage;
    
    
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
        SmashAction(parent);

    }

    void SmashAction(GameObject parent)
    {
        //yield return new WaitForSeconds(jumpingAnimationTime);
        
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(parent.transform.position, radius);
 
        foreach (Collider2D col in objectsInRange)
        {
            if (col.CompareTag("Enemy"))
            {
                Rigidbody2D rbEnemy = col.GetComponent<Rigidbody2D>();
                var dir = (col.transform.position - parent.transform.position);
                float wearoff = 1 - (dir.magnitude / radius);
                rbEnemy.velocity = dir.normalized * knockback * wearoff;
            }
        }
    }
}
