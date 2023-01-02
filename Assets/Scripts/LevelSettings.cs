using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public string sceneName;
    public GameState startingGameState;
    public string musicState;
    public Transform[] enemies;
    public Wave[] waves;
    public int currentWave = 0;

    public float timeBetweenWaves = 2f;

    public static LevelSettings GetCurrentLevelSettings()
    {
        return GameObject.FindObjectOfType<LevelSettings>();
    }
}