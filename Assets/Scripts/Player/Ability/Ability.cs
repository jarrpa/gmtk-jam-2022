using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public new String name;
    public float cooldown;
    public float activeTime;

    public FMODUnity.EventReference abilitySound;
    public FMOD.Studio.EventInstance soundInstance;

    public abstract void Activate(GameObject parent);
    public abstract void Deactivate();

    public virtual void PlayAbilitySound() {
        soundInstance = FMODUnity.RuntimeManager.CreateInstance(abilitySound);
        soundInstance.setParameterByNameWithLabel("AbilityKind", name);
        soundInstance.start();
    }
}
