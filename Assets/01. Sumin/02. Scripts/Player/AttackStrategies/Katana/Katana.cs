using UnityEngine;

public class Katana : MonoBehaviour
{
    [Header("# Hierarchy")]
    [SerializeField] private PlayerAttackHandler _player;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(_player.CurrentStrategy.GetStat(EStatType.Damage));
            Debug.Log(_player.CurrentStrategy.GetStat(EStatType.Damage));
        }
    }
}
