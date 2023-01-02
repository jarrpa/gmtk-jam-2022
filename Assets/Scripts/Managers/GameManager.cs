using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameIsPaused;

    public static event Action<GameState> OnGameStateChanged;

    public GameState gameState;

    public GameObject player;

    public EntityEvent entityDeathEvent;
    public GameEvent wavesDoneEvent;
    public GameEvent gameOverEvent;
    public GameEvent pauseEvent;
    public LevelEvent playerVictoryEvent;

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
        playerVictoryEvent ??= GameEventLoader.Load<LevelEvent>("PlayerVictoryLevelEvent");
        entityDeathEvent?.AddListener(OnDeath);
        wavesDoneEvent?.AddListener(OnLevelCleared);
        playerVictoryEvent?.AddListener(OnPlayerWin);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var levelSettings = LevelSettings.GetCurrentLevelSettings();
        var currentGameState = gameState;
        player = GameObject.FindWithTag("Player");

        if (levelSettings != null) {
            currentGameState = levelSettings.startingGameState;
        } else {
            if (player != null && scene.name != MenuManager.Instance.firstLevel)
                currentGameState = GameState.LevelStart;
        }

        UnpauseGame();
        UpdateGameState(currentGameState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsGameplayLevel())
            TogglePause();
    }

    public bool IsGameplayLevel()
    {
        return gameState == GameState.StartRoom || gameState == GameState.LevelStart || gameState == GameState.NextLevel;
    }

    private void OnDestroy()
    {
        entityDeathEvent?.RemoveListener(OnDeath);
        wavesDoneEvent?.RemoveListener(OnLevelCleared);
        playerVictoryEvent?.RemoveListener(OnPlayerWin);

        Time.timeScale = 1f;
    }

    public void UpdateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
                break;
            case GameState.StartRoom:
                break;
            case GameState.NextLevel:
                HandleNextLevel();
                break;
            case GameState.LevelStart:
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
        gameState = newState;
        OnGameStateChanged?.Invoke(gameState);
    }

    private void TogglePause()
    {
        if (gameIsPaused)
            UnpauseGame();
        else
            PauseGame();

        pauseEvent?.Invoke();
    }

    private void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
    }

    private void UnpauseGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
    }

    private void OnLevelCleared()
    {
        UpdateGameState(GameState.NextLevel);
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

    private void HandleNextLevel()
    {
        Singleton.Instance.WaveManager.StopWaves();
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

    private void OnPlayerWin(LevelPayload payload)
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
    StartRoom,
    LevelStart,
    NextLevel,
    Victory,
    Death,
}