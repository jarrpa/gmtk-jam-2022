using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent)), CanEditMultipleObjects]
public class GameEventEditor : Editor {
    public override void OnInspectorGUI() {
        var strings = new List<string>();
        var listeners = (target as GameEvent)?.GetListeners();
        DrawDefaultInspector();
        Rect r = EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginFoldoutHeaderGroup(true, "Registered Listeners");
        foreach (EventListener listener in listeners) {
            var listenerName = listener.Method.DeclaringType.ToString() + "." + listener.Method.Name;
            strings.Add(listenerName);
            EditorGUILayout.LabelField(listenerName);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.EndVertical();
    }
}

[CustomEditor(typeof(EntityEvent)), CanEditMultipleObjects]
public class EntityEventEditor : Editor {
    public override void OnInspectorGUI() {
        var strings = new List<string>();
        var listeners = (target as EntityEvent)?.GetListeners();
        DrawDefaultInspector();
        Rect r = EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginFoldoutHeaderGroup(true, "Registered Listeners");
        foreach (EventListener<EntityPayload> listener in listeners) {
            var listenerName = listener.Method.DeclaringType.ToString() + "." + listener.Method.Name;
            strings.Add(listenerName);
            EditorGUILayout.LabelField(listenerName);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        EditorGUILayout.EndVertical();
    }
}
