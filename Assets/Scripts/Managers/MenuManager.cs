using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private MenuManager Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void PlayGame(string level)
    {
        SceneManager.LoadScene(level);
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void ShowOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}