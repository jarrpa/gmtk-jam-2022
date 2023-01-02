using UnityEngine;

public class LevelGate : MonoBehaviour
{
    public string nextLevel;
    [SerializeField] private LevelEvent levelChangeEvent;
    [SerializeField] private GameEvent wavesDoneEvent;
    public bool isGateOpen;

    void Awake()
    {
        // Events we trigger
        levelChangeEvent ??= GameEventLoader.Load<LevelEvent>("LevelChangeEvent");

        // Events we listen
        wavesDoneEvent ??= GameEventLoader.Load<GameEvent>("WavesDoneEvent");
        wavesDoneEvent?.AddListener(OpenGate);
    }

    private void OpenGate()
    {
        foreach (var collider in GetComponents<BoxCollider2D>())
            if (!collider.isTrigger) collider.enabled = false;

        isGateOpen = true;
        var animator = GetComponent<Animator>();
        animator.SetBool("IsOpen", isGateOpen);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isGateOpen)
        {
            Debug.Log("NEXT LEVEL!");
            var payload = new LevelPayload();
            payload.levelSettings = Resources.Load<LevelSettings>("LevelSettings/" + nextLevel + "Settings");
            levelChangeEvent?.Invoke(payload);
        }
    }

    private void OnDestroy() {
        wavesDoneEvent?.RemoveListener(OpenGate);
    }
}
