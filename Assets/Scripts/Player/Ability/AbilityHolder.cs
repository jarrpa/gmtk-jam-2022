using System;
using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    private float cooldown;
    private float activeTime;

    enum AbilityState
    {
        Ready,
        Active,
        Cooldown
    }

    private AbilityState state = AbilityState.Ready;
    public KeyCode key;

    public void Update()
    {
        switch(state)
        {
            case AbilityState.Ready:
                if (Input.GetKeyDown(key))
                {
                    ability.Activate(gameObject);
                    state = AbilityState.Active;
                    activeTime = ability.activeTime;
                }
                break;
            case AbilityState.Active:
                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.Cooldown;
                    cooldown = ability.cooldown;
                }
                break;
            case AbilityState.Cooldown:
                if (cooldown > 0)
                {
                    cooldown -= Time.deltaTime;
                }
                else
                {
                    state = AbilityState.Ready;
                }
                break;
        }
    }

    public bool IsActive()
    {
        return state == AbilityState.Active;
    }
}
