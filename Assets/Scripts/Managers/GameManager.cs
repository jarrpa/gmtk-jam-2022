using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _gameIsPaused;

    public static event Action<GameState> OnGameStateChanged;
    
    private GameState _gameState;

    public GameObject player;

    public UnityEvent onPlayerReady;
    public UnityEvent onPlayerDeath;
    public UnityEvent<bool> onPause;

    private int _prevWaveCount;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        _gameState = GameState.LevelOne;
        _gameIsPaused = false;
        PauseGame();
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil( () => Singleton.Instance != null );
        player.GetComponent<Entity>().onDeath.AddListener(OnGameOver);
        Singleton.Instance.WaveManager.onWavesDone.AddListener(OnPlayerWin);
        UpdateGameState(GameState.LevelOne);
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.GetComponent<Entity>().onDeath.RemoveListener(OnGameOver);
            Singleton.Instance.WaveManager.onWavesDone.RemoveListener(OnPlayerWin);
        }
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
            case GameState.Pause:
                HandlePause();
                break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnGameStateChanged?.Invoke(_gameState);
    }

    private void PauseGame()
    {
        if (_gameIsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
        }
        onPause?.Invoke(_gameIsPaused);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && _gameState == GameState.LevelOne)
        {
            _gameIsPaused = !_gameIsPaused;
            PauseGame();
        }
    }
    
    private void HandlePause()
    {
        throw new NotImplementedException();
    }

    private void HandleGameOver()
    {
        onPlayerDeath?.Invoke();
        SetPlayerMovement(false);
        Singleton.Instance.WaveManager.StopWaves();
    }

    private void OnGameOver()
    {
        UpdateGameState(GameState.Death);
    }
    
    private void HandleVictory()
    {
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
    Pause,
}