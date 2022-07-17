using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    public new String name;
    public float cooldown;
    public float activeTime;

    public virtual void Activate(GameObject parent) {}
}
