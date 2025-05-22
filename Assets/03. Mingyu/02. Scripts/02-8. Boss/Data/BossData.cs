using UnityEngine;

[System.Serializable]
public class BossData
{
    private EBossType _bossType;
    public EBossType BossType { get => _bossType; set => _bossType = value; }

    [Header("Basic")] 
    public float MaxHealth;

    [SerializeField] 
    private float _currentHealth;
    public float CurrentHealth
    {
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0f, MaxHealth);
    }

    public float MoveSpeed;


    [Header("Attack")]
    public float AttackDamage;
    public float AttackCooltime;
    private float _lastAttackTime;
    public float LastAttackTime
    {
        get => _lastAttackTime;
        set => _lastAttackTime = value;
    }

}
