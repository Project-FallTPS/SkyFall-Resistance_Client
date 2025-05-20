using VInspector;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public EEnemyType EnemyType;

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

    [ShowIf("EnemyType", EEnemyType.Bombing)]
    public float ExplosionRadius;
}
