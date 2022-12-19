using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance { get; private set; }
    public GameManager GameManager { get; private set; }
    public WaveManager WaveManager { get; private set; }
    public UIManager UIManager { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Instance);

        GameManager = GetComponentInChildren<GameManager>();
        WaveManager = GetComponentInChildren<WaveManager>();
        UIManager = GetComponentInChildren<UIManager>();
    }
}