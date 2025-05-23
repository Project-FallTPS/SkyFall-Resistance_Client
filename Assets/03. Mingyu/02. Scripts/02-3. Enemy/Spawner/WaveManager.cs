using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WaveData
{
    public float EnemyStatMultiplier;
    public float EnemySpawnerInterval;
    public float WaveDuration; 
}

public class WaveManager : Singleton<WaveManager>
{
    [Header("Wave Data")]
    [SerializeField]
    private List<WaveData> _waveDatas = new List<WaveData>();

    private WaveData _currentWaveData;
    public WaveData CurrentWaveData 
    { 
        get => _currentWaveData; 
        set
        {
            _currentWaveData = value;
            _enemySpawnerHandler.AdjustSpawnerIntervalOnWave();
            // TODO : ���̺갡 �ٲ��� �����ִ� UI
        }
    }

    [Header("External References")]
    [SerializeField]
    private EnemySpawnerHandler _enemySpawnerHandler;

    private int _currentWaveIndex;
    private float _currentWaveStartTime;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        CurrentWaveData = _waveDatas[_currentWaveIndex];
    }

    private void Update()
    {
        if (_currentWaveStartTime + _waveDatas[_currentWaveIndex].WaveDuration <= Time.time)
        {
            ChangeWave();
        }
    }

    private void ChangeWave()       
    { 
        _currentWaveIndex++;
        if (_currentWaveIndex < _waveDatas.Count)
        {
            CurrentWaveData = _waveDatas[_currentWaveIndex];
            _currentWaveStartTime = Time.time;
        }
        else
        {
            _currentWaveIndex = 0;
            // TODO : 씬 전환
        }
    }
}
