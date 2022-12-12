using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Dash")]
public class DashAbility : Ability
{
    public float dashVelocity;
    public float knockback = 20f;
    public float radius = 1f;
    public int dashDamage;
    public float dashStunDuration;
    private Entity parentEntity;

    private AttackPayload attack = new AttackPayload();

    public override void Activate(GameObject parent)
    {
        parentEntity = parent.GetComponent<Entity>();
        parentEntity.isInvuln = true;

        attack.attacker = parentEntity;
        attack.damage = dashDamage;
        attack.isStun = true;
        attack.stunDuration = dashStunDuration;


        DashAction(parent);

        PlayAbilitySound();
    }

    public override void Deactivate()
    {
        parentEntity.isInvuln = false;
    }
    void DashAction(GameObject parent)
    {
        PlayerController movement = parent.GetComponent<PlayerController>();
        Rigidbody2D rb = parent.GetComponent<Rigidbody2D>();

        rb.AddForce(movement.GetVelocity() * dashVelocity, ForceMode2D.Impulse);
    }

    public void DashAttack(GameObject parent, GameObject target) {
        var dir = (target.transform.position - parent.transform.position);
        var rb = target.GetComponent<Rigidbody2D>();
        var entity = target.GetComponent<Entity>();
        rb.AddForce(dir.normalized * knockback, ForceMode2D.Impulse);
        entity.TakeDamage(attack);
    }
}
