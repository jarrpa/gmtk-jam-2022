using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Smash")]
public class SmashAbility : Ability
{
    public float knockback = 20f;
    public float radius = 2f;
    public int damage = 5;
    public float stunDuration = 1.5f;
    private AttackPayload attack = new AttackPayload();


    public override void Activate(GameObject parent)
    {
        attack.attacker = parent.GetComponent<Entity>();
        attack.damage = damage;
        attack.isStun = true;
        attack.stunDuration = stunDuration;

        SmashAction(parent);
        PlayAbilitySound();
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
                col.GetComponent<Rigidbody2D>().AddForce(dir.normalized * knockback);
                
                col.GetComponent<Entity>().TakeDamage(attack);
            }
        }
    }
}
