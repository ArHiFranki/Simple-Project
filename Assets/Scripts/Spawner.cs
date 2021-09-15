using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<Wave> _waves;
    [SerializeField] private List<Transform> _spawnPoints;

    private Wave _currentWave;
    private int _currentWaveNumber = 0;
    private int _spawned;
    private float _timeAfterLastSpawn;

    public event UnityAction AllEnemySpawned;

    private void Start()
    {
        SetWave(_currentWaveNumber);
    }

    private void Update()
    {
        if (_currentWave == null)
        {
            return;
        }

        _timeAfterLastSpawn += Time.deltaTime;

        if (_timeAfterLastSpawn >= _currentWave.Delay)
        {
            InstantiateEnemy();
            _spawned++;
            _timeAfterLastSpawn = 0;
        }

        if (_currentWave.Count <= _spawned)
        {
            if (_waves.Count > _currentWaveNumber + 1)
            {
                AllEnemySpawned?.Invoke();
            }

            _currentWave = null;
        }
    }

    private void InstantiateEnemy()
    {
        int randomNumber = Random.Range(0, _spawnPoints.Count);
        Instantiate(_currentWave.Template, 
                    _spawnPoints[randomNumber].position, 
                    _spawnPoints[randomNumber].rotation, 
                    _spawnPoints[randomNumber]);
    }

    private void SetWave(int index)
    {
        _currentWave = _waves[index];
    }

    public void StartNextWave()
    {
        SetWave(++_currentWaveNumber);
        _spawned = 0;
    }
}

[System.Serializable]
public class Wave
{
    public GameObject Template;
    public float Delay;
    public int Count;
}
