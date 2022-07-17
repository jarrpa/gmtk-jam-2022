using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public enum SpawnState
    {
        spawning,
        waiting,
        counting
    };


    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    private int nextWave = 0;
    
    public float timeBetweenWaves = 4f;
    private float waveCountDown;

    private float searchTimer;
    private float searchCountDown;
    private SpawnState state = SpawnState.counting;
    private void Start()
    {
        waveCountDown = timeBetweenWaves;
        if (spawnPoints.Length == 0)
        {
            Debug.Log("Error no spawn Points set");
        }
    }
    private void Update()
    {
        if (state == SpawnState.waiting)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountDown <= 0)
        {
            if (state != SpawnState.spawning)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }

    private bool EnemyIsAlive()
    {
        searchCountDown = searchTimer;
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            searchCountDown = searchTimer;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {

                return false;
            }
        }
        
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave: " + _wave.name);
        state = SpawnState.spawning;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.waiting;
        yield break;
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave Complete");
        state = SpawnState.counting;
        waveCountDown = timeBetweenWaves;

        if (nextWave ++ > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("All Waves Complete. Looping ..."); 
            // Call to start new scene here
        }
        else
        {
            nextWave++;
        }
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy: " + _enemy.name);
        
        // Selects a random spawn point; 
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation); 
        
    }
}
