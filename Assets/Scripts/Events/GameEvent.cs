using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using UnityEngine.UIElements;

public delegate void EventListener();
public delegate void EventListener<T>(T arg);

public static class GameEventLoader {
    public static T Load<T>(string assetName) where T : UnityEngine.Object {
        return Resources.Load<T>("Events/" + assetName);
    }
}

[CreateAssetMenu(fileName = "GameEvent", menuName = "Events/GameEvent")]
public class GameEvent : ScriptableObject //, ISerializationCallbackReceiver
{
    [SerializeField] private List<EventListener> listeners = new List<EventListener>();

    public void Invoke() {
        for (int i = 0; i < listeners.Count; i++) {
            listeners[i].Invoke();
        }
    }

    public void AddListener(EventListener listener) {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void RemoveListener(EventListener listener) {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }

    public List <EventListener> GetListeners() {
        return listeners;
    }
}

[Serializable]
public class GameEvent<T> : ScriptableObject //, ISerializationCallbackReceiver
{
    [SerializeField] private List<EventListener<T>> listeners = new List<EventListener<T>>();

    public void SomeAction(T input) {}

    public void Invoke(T payload) {
        for (int i = 0; i < listeners.Count; i++) {
            listeners[i](payload);
        }
    }

    public void AddListener(EventListener<T> listener) {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void RemoveListener(EventListener<T> listener) {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }

    public List <EventListener<T>> GetListeners() {
        return listeners;
    }
}