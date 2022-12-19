using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameIsPaused;

    public static event Action<GameState> OnGameStateChanged;

    private GameState _gameState;

    public GameObject player;

    public EntityEvent entityDeathEvent;
    public GameEvent wavesDoneEvent;
    public GameEvent gameOverEvent;
    public GameEvent pauseEvent;

    private int _prevWaveCount;

    private static GameManager Instance;

    // Self-initialization with no references to other GameObjects
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;

        // Events we trigger
        gameOverEvent ??= GameEventLoader.Load<GameEvent>("GameOverEvent");
        pauseEvent ??= GameEventLoader.Load<GameEvent>("PauseEvent");

        // Events we listen
        entityDeathEvent ??= GameEventLoader.Load<EntityEvent>("EntityDeathEvent");
        wavesDoneEvent ??= GameEventLoader.Load<GameEvent>("WavesDoneEvent");
        entityDeathEvent?.AddListener(OnDeath);
        wavesDoneEvent?.AddListener(OnPlayerWin);

        _gameState = GameState.LevelOne;
        gameIsPaused = false;
        PauseGame();
    }

    private void Start()
    {
        UpdateGameState(GameState.LevelOne);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && _gameState == GameState.LevelOne)
        {
            gameIsPaused = !gameIsPaused;
            PauseGame();
        }
    }

    private void OnDestroy()
    {
        entityDeathEvent?.RemoveListener(OnDeath);
        wavesDoneEvent?.RemoveListener(OnPlayerWin);

        Time.timeScale = 1f;
    }

    public void UpdateGameState(GameState newState)
    {
        _gameState = newState;
        switch (newState)
        {
            case GameState.LevelOne:
                StartCoroutine(HandleLevelOne());
                break;
            case GameState.Victory:
                HandleVictory();
                break;
            case GameState.Death:
                HandleGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(_gameState);
    }

    private void PauseGame()
    {
        if (gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
        pauseEvent?.Invoke();
    }

    private void HandleGameOver()
    {
        gameOverEvent?.Invoke();
        SetPlayerMovement(false);
        Singleton.Instance.WaveManager.StopWaves();
    }

    private void OnDeath(EntityPayload entityPayload)
    {
        if (entityPayload.entity.CompareTag("Player"))
            UpdateGameState(GameState.Death);
    }

    private void HandleVictory()
    {
        SetPlayerMovement(false);
        Singleton.Instance.WaveManager.StopWaves();
    }

    IEnumerator HandleLevelOne()
    {
        player.GetComponent<PlayerAbilityController>().ChoseAbilities();
        yield return new WaitForSeconds(2f);
        Singleton.Instance.WaveManager.StartWaves();
    }

    private void SetPlayerMovement(bool enable)
    {
        player.GetComponent<PlayerController>().enabled = enable;
        player.GetComponent<PlayerAim>().enabled = enable;
        player.GetComponent<AbilityHolder>().enabled = enable;
        player.GetComponentInChildren<Weapon>().enabled = enable;
    }

    private void OnPlayerWin()
    {
        UpdateGameState(GameState.Victory);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum GameState
{
    MainMenu,
    LevelOne,
    NextLevel,
    Victory,
    Death,
}