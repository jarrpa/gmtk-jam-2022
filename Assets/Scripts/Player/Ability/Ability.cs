using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public new String name;
    public float cooldown;
    public float activeTime;

    public abstract void Activate(GameObject parent);
    public abstract void Deactivate();
}
