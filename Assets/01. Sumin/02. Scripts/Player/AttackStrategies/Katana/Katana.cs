using UnityEngine;

public class Katana : MonoBehaviour
{
    [Header("# Hierarchy")]
    [SerializeField] private PlayerAttackHandler _player;
    
    private void OnTriggerEnter(Collider other)
    {
        if(!GetComponent<Collider>().enabled)
        {
            return;
        }    
        if(!other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var damageable))
        {
            float baseDamage = _player.CurrentStrategy.GetStat(EStatType.Damage);
            damageable.TakeDamage(baseDamage);
            
            // 액세서리 이벤트 실행
            if (_player.CurrentStrategy is KatanaStrategy katanaStrategy)
            {
                katanaStrategy.AccessoryOnAttack();
                
                // 각 액세서리의 OnHit 이벤트 호출
                foreach (var acc in AccessoryManager.Instance.EquippedAccessories)
                {
                    acc.Value.Object.OnHit(damageable, baseDamage);
                }
            }
        }
    }
}