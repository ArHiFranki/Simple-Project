using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private List<Transform> _spawnPoints;

    private Wave _currentWave;
    private int _currentWaveEnemyCount;
    private int _currentWaveNumber = 0;
    private int _spawned;
    private float _timeAfterLastSpawn;

    public int CurrentWaveEnemyCount => _currentWaveEnemyCount;

    public event UnityAction AllEnemyInCurrentWaveSpawned;

    private void Start()
    {
        SetWave(_currentWaveNumber);
        TotalEnemyCount();
        _currentWaveEnemyCount = 0;
    }

    private void Update()
    {
        if (_currentWave == null)
        {
            return;
        }

        SpawnWave();
    }

    private void InstantiateEnemy()
    {
        int randomNumber = Random.Range(0, _spawnPoints.Count);
        Enemy tmpEnemy = Instantiate(_currentWave.Template, 
                                     _spawnPoints[randomNumber].position, 
                                     _spawnPoints[randomNumber].rotation, 
                                     _spawnPoints[randomNumber]);
        tmpEnemy.InitUnit(_gameController);
    }

    private void SetWave(int index)
    {
        _currentWave = _waves[index];
    }

    public void StartNextWave()
    {
        SetWave(_currentWaveNumber++);
        _spawned = 0;
        _currentWaveEnemyCount = _currentWave.Count;
        Debug.Log("_currentWaveEnemyCount: " + _currentWaveEnemyCount);
    }

    private void SpawnWave()
    {
        if(_gameController.IsFightPhase && !_gameController.IsGameOver)
        {
            _timeAfterLastSpawn += Time.deltaTime;

            if (_timeAfterLastSpawn >= _currentWave.Delay)
            {
                InstantiateEnemy();
                _spawned++;
                _timeAfterLastSpawn = 0;
            }

            if (_spawned == _currentWave.Count)
            {
                AllEnemyInCurrentWaveSpawned?.Invoke();
                _currentWave = null;
            }
        }
    }

    private void TotalEnemyCount()
    {
        int totalEnemysCount = 0;

        for (int i = 0; i < _waves.Count; i++)
        {
            totalEnemysCount += _waves[i].Count;
        }

        _gameController.SetTotalEnemyCount(totalEnemysCount);
    }
}

[System.Serializable]
public class Wave
{
    public Enemy Template;
    public float Delay;
    public int Count;
}
