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

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        wavesText.text = "0 / " + Singleton.Instance.WaveManager.waves.Length;
        Singleton.Instance.WaveManager.onWaveChange.AddListener(UpdateWaveCount);
        Singleton.Instance.GameManager.onPause.AddListener(PauseGame);
        Singleton.Instance.GameManager.onPlayerDeath.AddListener(GameOverPanel);
        Singleton.Instance.WaveManager.onWavesDone.AddListener(WinPanel);
        pauseTitleImage = pauseTitle.GetComponent<Image>();
    }

    private void OnEnable()
    {
        player.GetComponent<PlayerAbilityController>().OnAbilityChange += PlayCardAnimations;
        player.GetComponent<Entity>().onHit.AddListener(UpdatePlayerHealth);
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.GetComponent<PlayerAbilityController>().OnAbilityChange -= PlayCardAnimations;
            player.GetComponent<Entity>().onHit.RemoveListener(UpdatePlayerHealth);
        }
        Singleton.Instance.WaveManager.onWaveChange.RemoveListener(UpdateWaveCount);
        Singleton.Instance.GameManager.onPause.RemoveListener(PauseGame);
        Singleton.Instance.GameManager.onPlayerDeath.RemoveListener(GameOverPanel);
        Singleton.Instance.WaveManager.onWavesDone.RemoveListener(WinPanel);
        Time.timeScale = 1f;
    }

    private void UpdatePlayerHealth(int health)
    {
        for (int i = 100; i >= 0; i -= 20)
        {
            if (health > i - 20 && health < i)
            {
                healthImage.sprite = healthSprites[((100 - i) / 20)%healthSprites.Length];
            }
        }
    }

    private void UpdateWaveCount(int count)
    {
        wavesText.text = count+1 + " / " + Singleton.Instance.WaveManager.waves.Length;
    }

    private void PlayCardAnimations(int ability, int weapon)
    {
        attackCardAnimator.Play("card_" + weapon);
        moveCardAnimator.Play("card_" + ability);
    }

    private void PauseGame(bool pause)
    {
        pauseTitleImage.sprite = pauseTitleSprites[0];
        pausePanel.SetActive(pause);
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
