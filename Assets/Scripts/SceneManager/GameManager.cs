using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake() {
        print("GameManager Awake");
        UnityEngine.SceneManagement.SceneManager.LoadScene("SebastianScene2");
        // if user presses the escape key change scene to SebastianScene2
        
    }
}