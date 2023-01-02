using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameEvent pickupEvent;

    public virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            pickupEvent?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
