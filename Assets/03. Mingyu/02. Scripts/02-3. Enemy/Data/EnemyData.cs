using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public EEnemyType EnemyType;
    public EDamageableType DamageableType;

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
    public float AttackableRange;
    public float AttackDamage;
    public float AttackDelay;
    
    private float _nextAttackableTime;
    public float NextAttackableTime { get => _nextAttackableTime; set => _nextAttackableTime = value; }
    
    [Header("Bombing Type")]
    public float ExplosionRadius;
    
    [Header("Item Drop")]
    public float AccessoryBoxDropProbability;

    [Header("Shield")] 
    private bool _isShieldActive = false;
    public bool IsShieldActive { get => _isShieldActive; set => _isShieldActive = value; }

    public float ShieldBuffRadius;

    public void AdjustEnemyDataOnWave(float multiplier)
    {
        MaxHealth *= multiplier;
        CurrentHealth *= multiplier;
        AttackDamage *= multiplier;
    }

    public void Init()
    {
        _nextAttackableTime = Time.time;
        _currentHealth = MaxHealth;
        _isShieldActive = false;
    }
    public EnemyData(EnemyData original)
    {
        EnemyType = original.EnemyType;
        DamageableType = original.DamageableType;
        
        MaxHealth = original.MaxHealth;
        MoveSpeed = original.MoveSpeed;
        
        AttackableRange = original.AttackableRange;
        AttackDamage = original.AttackDamage;
        AttackDelay = original.AttackDelay;
        
        NextAttackableTime = original.NextAttackableTime;
        ExplosionRadius = original.ExplosionRadius;
        AccessoryBoxDropProbability = original.AccessoryBoxDropProbability;
        IsShieldActive = original.IsShieldActive;
        ShieldBuffRadius = original.ShieldBuffRadius;
    }
}
