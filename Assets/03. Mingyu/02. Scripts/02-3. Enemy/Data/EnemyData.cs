using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public EEnemyType EnemyType;
    public EDamageableType DamageableType;

    [Header("Basic")]
    public float MaxHealth;

    [SerializeField]
    private float _currentHealthPoint;
    public float CurrentHealth 
    { 
        get => _currentHealthPoint;
        set => _currentHealthPoint = Mathf.Clamp(value, 0f, MaxHealth); 
    }

    public float MoveSpeed;

    [Header("Attack")]
    public float AttackableRange;
    public float AttackDamage;
    public float AttackDelay;
    
    [Header("Bombing Type")]
    public float ExplosionRadius;
    
    [Header("Item Drop")]
    public float AccessoryBoxDropProbability;

    public void AdjustEnemyDataOnWave(float multiplier)
    {
        MaxHealth *= multiplier;
        CurrentHealth *= multiplier;
        AttackDamage *= multiplier;
    }
}
