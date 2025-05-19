using VInspector;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    [SerializeField]
    private EEnemyType _enemyType;
    public EEnemyType EnemyType { get => _enemyType; set => _enemyType = value; }

    [Header("Basic Stat")]
    [SerializeField]
    private float _maxHealthPoint = 100f;
    public float MaxHealthPoint { get => _maxHealthPoint; set => _maxHealthPoint = value; }

    [SerializeField]
    private float _currentHealthPoint;
    public float CurrentHealthPoint 
    { 
        get => _currentHealthPoint;
        set => _currentHealthPoint = Mathf.Clamp(value, 0f, _maxHealthPoint); 
    }

    [SerializeField]
    private float _movementSpeed = 5f;
    public float MovementSpeed { get => _movementSpeed; set => _movementSpeed = value; }


    // [Header("Trace State")]

    [Foldout("Attack State")]
    [SerializeField]
    private float _attackableRange;
    public float AttackableRange { get => _attackableRange; set => _attackableRange = value; }
    [SerializeField]
    private float _attackDamage = 1f;
    public float AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    [Header("Attack Shooting")]
    [SerializeField]
    private float _attackDelay = 5f;
    public float AttackDelay { get => _attackDelay; set => _attackDelay = value; }
    [Header("Attack Bombing")]
    [SerializeField]
    private float _bombRange = 3f;

    // [Header("Damaged State")]

    // [Header("Die State")]
}
