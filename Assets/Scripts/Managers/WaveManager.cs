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
    public int currentWave = 0;
    
    public float timeBetweenWaves = 4f;
    private float _waveCountDown;

    private float _searchTimer;
    private float _searchCountDown;
    private SpawnState _state = SpawnState.Inactive;

    public GameEvent waveChangeEvent;
    public GameEvent wavesDoneEvent;

    private static WaveManager Instance;

    // Self-initialization with no references to other GameObjects
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            DestroyImmediate(this);
            return;
        }
        Instance = this;

        _waveCountDown = timeBetweenWaves;

        // Events we trigger
        waveChangeEvent ??= GameEventLoader.Load<GameEvent>("WaveChangeEvent");
        wavesDoneEvent ??= GameEventLoader.Load<GameEvent>("WavesDoneEvent");
    }
    private void Update()
    { 
        if(_state == SpawnState.Inactive)
        {
            return;
        }
        
        if (_state == SpawnState.Waiting)
        {
            if (!EnemyIsAlive())
                WaveCompleted();
            else
                return;
        }

        if (_state == SpawnState.Counting)
        {
            if (_waveCountDown > 0)
            {
                _waveCountDown -= Time.deltaTime;
            }
            else if (_state != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[currentWave - 1]));
            }
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
        Debug.Log("Wave Complete, wave number was " + currentWave);
        
        _state = SpawnState.Counting;
        _waveCountDown = timeBetweenWaves;

        if (currentWave >= waves.Length)
        {
            _state = SpawnState.Inactive;
            wavesDoneEvent?.Invoke();
        }
        else
        {
            currentWave++;
            waveChangeEvent?.Invoke();
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
        currentWave = 1;
        spawnPoints.Clear();

        var spawnPointObjects = GameObject.FindGameObjectsWithTag("SpawnPoint");
        Debug.Log("Found points: " + spawnPointObjects.Length);
        for (int i = 0; i < spawnPointObjects.Length; i++)
        {
            Debug.Log("add : " + spawnPointObjects[i].name);
            spawnPoints.Add(spawnPointObjects[i].transform);
        }

        waveChangeEvent?.Invoke();
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
