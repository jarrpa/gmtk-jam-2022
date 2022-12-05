using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    public enum SpawnState
    {
        Inactive,
        Spawning,
        Waiting,
        Counting
    };


    [System.Serializable]
    public class Wave
    {
        public string name;
        public int[] counts;
        public float rate;
    }

    public Transform[] enemies;
    public Wave[] waves;
    public List<Transform> spawnPoints;
    private int _nextWave;
    
    public float timeBetweenWaves = 4f;
    private float _waveCountDown;

    private float _searchTimer;
    private float _searchCountDown;
    private SpawnState _state = SpawnState.Inactive;

    public UnityEvent<int> onWaveChange;
    public UnityEvent onWavesDone;

    private void Start()
    {
        _waveCountDown = timeBetweenWaves;
    }
    private void Update()
    { 
        if(_state == SpawnState.Inactive)
        {
            // Debug.Log("WaveManager Inactive");
            return;
        }
        
        if (_state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive())
                WaveCompleted();
            else
                return;
        }

        if (_waveCountDown <= 0)
        {
            if (_state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[_nextWave]));
            }
        }
        else
        {
            _waveCountDown -= Time.deltaTime;
        }
    }

    private bool EnemyIsAlive()
    {
        _searchCountDown = _searchTimer;
        _searchCountDown -= Time.deltaTime;
        if (_searchCountDown <= 0f)
        {
            _searchCountDown = _searchTimer;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        
        return true;
    }

    IEnumerator SpawnWave(Wave wave)
    {
        Debug.Log("Spawning Wave: " + wave.name);
        _state = SpawnState.Spawning;

        for (int i = 0; i < enemies.Length; i++)
        {
            for (int count = 0; count < wave.counts[i]; count++)
            {
                SpawnEnemy(enemies[i]);
                yield return new WaitForSeconds(1f / wave.rate);
            }
        }

        _state = SpawnState.Waiting;
    }

    private void WaveCompleted()
    {
        Debug.Log("Wave Complete, wave number was " + _nextWave);
        
        _state = SpawnState.Counting;
        _waveCountDown = timeBetweenWaves;

        if (_nextWave + 1 >= waves.Length)
        {
            onWavesDone?.Invoke();
            _state = SpawnState.Inactive;
        }
        else
        {
            _nextWave++;
            onWaveChange?.Invoke(_nextWave);
        }
    }
    
    void SpawnEnemy(Transform enemy)
    {
        Debug.Log("Spawning Enemy: " + enemy.name);
        
        // Selects a random spawn point; 
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Instantiate(enemy, sp.position, sp.rotation);
    }

    public void StartWaves()
    {
        _nextWave = 0;
        spawnPoints.Clear();

        var spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Debug.Log("Found points: " + spawnPointObjects.Length);
        for (int i = 0; i < spawnPointObjects.Length; i++)
        {
            Debug.Log("add : " + spawnPointObjects[i].name);
            spawnPoints.Add(spawnPointObjects[i].transform);
        }

        onWaveChange?.Invoke(_nextWave);
        _state = SpawnState.Counting;
    }

    public void StopWaves()
    {
        _state = SpawnState.Inactive;
        var leftEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in leftEnemies)
            e.gameObject.SetActive(false);
    }
}
