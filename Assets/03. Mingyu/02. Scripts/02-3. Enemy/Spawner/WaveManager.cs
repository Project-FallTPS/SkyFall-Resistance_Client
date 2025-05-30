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
    private float _fallSceneDuration;
    public float FallSceneDuration
    {
        get => _fallSceneDuration;
        set => _fallSceneDuration = value;
    }

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
            // TODO : 웨이브가 바뀌었음을 알리는 UI 정도?
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
        CalculateFallSceneDuration();
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

    private void CalculateFallSceneDuration()
    {
        foreach (WaveData wave in _waveDatas)
        {
            _fallSceneDuration += wave.WaveDuration;
        }
    }
    
    private void ChangeWave()       
    { 
        _currentWaveIndex++;
        Debug.Log($"Change Wave! 현재 웨이브 : {_currentWaveIndex}, 현재 시간 : {Time.time}");
        if (_currentWaveIndex < _waveDatas.Count)
        {
            CurrentWaveData = _waveDatas[_currentWaveIndex];
            _currentWaveStartTime = Time.time;
        }
        else
        {
            _currentWaveIndex = 0;
            SceneTransitionManager.Instance.LoadScene(nameof(ESceneNames.BossEntryCutScene));
        }
    }
}
