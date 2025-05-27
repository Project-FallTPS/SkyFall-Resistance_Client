using System;
using UnityEngine;

public class PhaseManager : Singleton<PhaseManager>
{
    [SerializeField]
    private BossController _bossController;

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

    public Action<int> OnChangePhase;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        _maxPhase = 
            _bossController.BossData.MaxPhase;
        _currentPhase = 
            _bossController.BossData.CurrentPhase = 1;

        _bossController.BossData.OnPhaseChanged += ChangePhase;
    }

    private void ChangePhase()
    {
        _currentPhase = _bossController.BossData.CurrentPhase;
        OnChangePhase?.Invoke(_currentPhase);
    }
}
