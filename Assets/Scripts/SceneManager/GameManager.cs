using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameObject Manager;
    // Start is called before the first frame update
    void Awake() {
        print("GameManager Awake");
        Manager = GameObject.Find("GameManager");
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(Manager);
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Map1");
        }
    }
}