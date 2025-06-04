using UnityEngine;

public class Katana : MonoBehaviour
{
    [Header("# Hierarchy")]
    [SerializeField] private PlayerAttackHandler _player;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void EnableAttack()
    {
        _collider.enabled = true;

        // OnAttack 시점 처리
        if (_player.CurrentStrategy is KatanaStrategy katanaStrategy)
        {
            katanaStrategy.AccessoryOnAttack(); // 필요한 초기화 로직
        }

        SoundManager.Instance.PlaySfx(ESfxType.PlayerSword1);
    }

    public void DisableAttack()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_collider.enabled)
            return;

        if (!other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var damageable))
        {
            float baseDamage = _player.CurrentStrategy.GetStat(EStatType.Damage);
            damageable.TakeDamage(baseDamage);
            UIEventHandler.Instance.OnPlayerAttackHit?.Invoke();
            PlayerEffectPoolManager.Instance.GetObject(EPlayerEffectType.KatanaHitEffect, damageable.GameObject.transform.position, Quaternion.identity);

            foreach (var acc in AccessoryManager.Instance.EquippedAccessories)
            {
                acc.Value.Object.OnHit(damageable);
            }
        }
    }
}