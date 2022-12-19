using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator attackCardAnimator;
    public Animator moveCardAnimator;
    public GameObject player;

    public Sprite[] healthSprites;

    public Image healthImage;

    public TMP_Text wavesText;

    public GameObject pausePanel;
    public GameObject pauseTitle;
    private Image pauseTitleImage;
    public Sprite[] pauseTitleSprites;
    public GameEvent gameOverEvent;
    public GameEvent pauseEvent;
    public GameEvent waveChangeEvent;
    public GameEvent wavesDoneEvent;
    public EntityEvent entityHitEvent;

    private static UIManager Instance;

    // Self-initialization with no references to other GameObjects
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;

        // Events we listen
        waveChangeEvent ??= GameEventLoader.Load<GameEvent>("WaveChangeEvent");
        wavesDoneEvent ??= GameEventLoader.Load<GameEvent>("WavesDoneEvent");
        gameOverEvent ??= GameEventLoader.Load<GameEvent>("GameOverEvent");
        pauseEvent ??= GameEventLoader.Load<GameEvent>("PauseEvent");
        entityHitEvent ??= GameEventLoader.Load<EntityEvent>("EntityHitEvent");

        waveChangeEvent?.AddListener(UpdateWaveCount);
        wavesDoneEvent?.AddListener(WinPanel);
        gameOverEvent?.AddListener(GameOverPanel);
        pauseEvent?.AddListener(PauseGame);
        entityHitEvent.AddListener(UpdatePlayerHealth);

        wavesText.text = "0 / " + Singleton.Instance.WaveManager.waves.Length;
        pauseTitleImage = pauseTitle.GetComponent<Image>();
    }

    private void OnEnable()
    {
        player.GetComponent<PlayerAbilityController>().OnAbilityChange += PlayCardAnimations;
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.GetComponent<PlayerAbilityController>().OnAbilityChange -= PlayCardAnimations;
        }
        waveChangeEvent?.RemoveListener(UpdateWaveCount);
        wavesDoneEvent?.RemoveListener(WinPanel);
        gameOverEvent?.RemoveListener(GameOverPanel);
        pauseEvent?.RemoveListener(PauseGame);
        entityHitEvent.RemoveListener(UpdatePlayerHealth);

        Time.timeScale = 1f;
    }

    private void UpdatePlayerHealth(EntityPayload hitData)
    {
        if (hitData.entity.CompareTag("Player"))
        {
            var health = hitData.entity.currentHealth;
            for (int i = 100; i >= 0; i -= 20)
            {
                if (health > i - 20 && health < i)
                {
                    healthImage.sprite = healthSprites[((100 - i) / 20) % healthSprites.Length];
                }
            }
        }
    }

    private void UpdateWaveCount()
    {
        wavesText.text = Singleton.Instance.WaveManager.currentWave + " / " + Singleton.Instance.WaveManager.waves.Length;
    }

    private void PlayCardAnimations(int ability, int weapon)
    {
        attackCardAnimator.Play("card_" + weapon);
        moveCardAnimator.Play("card_" + ability);
    }

    private void PauseGame()
    {
        var isPaused = Singleton.Instance.GameManager.gameIsPaused;
        pauseTitleImage.sprite = pauseTitleSprites[0];
        pausePanel.SetActive(isPaused);
    }

    private void GameOverPanel()
    {
        pauseTitleImage.sprite = pauseTitleSprites[1];
        pausePanel.SetActive(true);
    }

    private void WinPanel()
    {
        pauseTitleImage.sprite = pauseTitleSprites[2];
        pausePanel.SetActive(true);
    }


    public void Exit()
    {

    }

}
