using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityEvent", menuName = "Events/EntityEvent")]
[Serializable]
public class EntityEvent : GameEvent<EntityPayload> {
}

[Serializable]
public struct EntityPayload {
    public Entity entity;
    public int damage;
}