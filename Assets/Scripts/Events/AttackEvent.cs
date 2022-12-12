using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackEvent", menuName = "Events/AttackEvent")]
[Serializable]
public class AttackEvent : GameEvent<AttackPayload> {
}

[Serializable]
public struct AttackPayload {
    public Entity attacker;
    public WeaponKind kind;
    public int damage;
    public bool isStun;
    public float stunDuration;
}