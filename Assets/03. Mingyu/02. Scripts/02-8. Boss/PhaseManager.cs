using System;
using UnityEngine;

public class PhaseManager : Singleton<PhaseManager>
{
    [SerializeField]
    private GameObject _bossPrefab;

    private int _maxPhase;
    public int MaxPhase
    {
        get => _maxPhase;
        set => _maxPhase = value;
    }
    
    private int _currentPhase;
    public int CurrentPhase
    {
        get => _currentPhase;
        set => _currentPhase = value;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _maxPhase = 
            _bossPrefab.GetComponent<BossController>().BossData.MaxPhase;
        _currentPhase = 
            _bossPrefab.GetComponent<BossController>().BossData.CurrentPhase = 3;
    }
}
