using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    private void GameManagerOnOnGameStateChanged(GameState state)
    {
        if (state == GameState.MainMenu)
        {
            
        }
    }
    // Start is called before the first frame update
    public void PlayGame() {
        SceneManager.LoadScene("Map_1");
    }

    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
