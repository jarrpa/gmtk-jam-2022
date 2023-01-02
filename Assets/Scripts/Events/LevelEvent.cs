using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelEvent", menuName = "Events/LevelEvent")]
[Serializable]
public class LevelEvent : GameEvent<LevelPayload> {
}

[Serializable]
public struct LevelPayload {
    public LevelSettings levelSettings;
}
