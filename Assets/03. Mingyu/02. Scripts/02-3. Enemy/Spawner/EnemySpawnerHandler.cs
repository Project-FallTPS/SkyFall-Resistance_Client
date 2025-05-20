using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerHandler : MonoBehaviour
{
    private List<EnemySpawner> _enemySpawners = new List<EnemySpawner>();
    public List<EnemySpawner> EnemySpawners { get => _enemySpawners; set => _enemySpawners = value; }

    private void Awake()
    {
        GetAllEnemySpawners();
    }

    private void GetAllEnemySpawners()
    {
        EnemySpawner[] enemySpawners = FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);
        foreach (var spawner in enemySpawners)
        {
            _enemySpawners.Add(spawner);
        }
    }

    public void AdjustSpawnerIntervalOnWave()
    {
        foreach (var spawner in _enemySpawners)
        {
            spawner.SpawnInterval = WaveManager.Instance.CurrentWaveData.EnemySpawnerInterval;
        }
    }
}
