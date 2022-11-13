using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionDoor : MonoBehaviour
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
        if (state != GameState.NextLevel)
        {
            gameObject.SetActive(false);
        }
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Next Scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
        }
    }
}
