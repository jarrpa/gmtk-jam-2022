using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaPickup : Pickup
{
    [SerializeField] private GameEvent wavesDoneEvent;

    void Awake()
    {
        // Events we trigger
        pickupEvent ??= GameEventLoader.Load<GameEvent>("BazookaPickupEvent");
        wavesDoneEvent ??= GameEventLoader.Load<GameEvent>("WavesDoneEvent");
    }

    public override void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) wavesDoneEvent?.Invoke();
        base.OnTriggerEnter2D(other);
    }
}
