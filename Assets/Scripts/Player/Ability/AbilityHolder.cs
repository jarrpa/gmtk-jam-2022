using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    private float _cooldown;
    private float _activeTime;

    public Material material;

    public ParticleSystem smashParticle;

    enum AbilityState
    {
        Ready,
        Active,
        Cooldown
    }

    private AbilityState _state = AbilityState.Ready;
    public KeyCode key;
    private static readonly int GrayscaleAmount = Shader.PropertyToID("_GrayscaleAmount");

    public void Update()
    {
        switch (_state)
        {
            case AbilityState.Ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate(gameObject);
                    _state = AbilityState.Active;
                    _activeTime = ability.activeTime;
                    switch (ability.name)
                    {
                        case "Smash":
                            smashParticle.Play();
                            break;
                        case "Teleport":
                            material.SetFloat(GrayscaleAmount, 1);
                            break;
                    }
                }
                break;
            case AbilityState.Active:
                if (_activeTime > 0)
                {
                    _activeTime -= Time.deltaTime;
                    if (ability.name.Equals("Teleport") && Input.GetKeyDown(key))
                    {
                        var teleportAbility = (TeleportAbility)ability;
                        teleportAbility.KeepActive(gameObject);
                        _activeTime = 0;
                    }
                }
                else
                {
                    ability.Deactivate();
                    if (ability.name.Equals("Teleport"))
                        material.SetFloat(GrayscaleAmount, 0);

                    _state = AbilityState.Cooldown;
                    _cooldown = ability.cooldown;
                }
                break;
            case AbilityState.Cooldown:
                if (_cooldown > 0)
                {
                    _cooldown -= Time.deltaTime;
                }
                else
                {
                    _state = AbilityState.Ready;
                }
                break;
        }
    }

    public bool IsActive()
    {
        return _state == AbilityState.Active;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (ability.name.Equals("Dash") && IsActive())
        {
            var dash = (ability as DashAbility);
            if (collision.gameObject == null || collision.gameObject.CompareTag(gameObject.tag))
                return;
            var entity = collision.gameObject.GetComponent<Entity>();
            if (entity != null)
            {
                dash.DashAttack(gameObject, collision.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {

        switch (ability.name)
        {
            case "Smash":
                var smashAbility = (SmashAbility)ability;
                Gizmos.DrawWireSphere(gameObject.transform.position, smashAbility.radius);
                break;
            case "Teleport":
                var teleportAbility = (TeleportAbility)ability;
                Gizmos.DrawWireSphere(gameObject.transform.position, teleportAbility.radius);
                break;
        }
    }
}
