using UnityEngine;

public class Acc_KatanaBomb : AccessoryBase, IAccessory
{
    [SerializeField] private GameObject _bombEffectPrefab;
    [SerializeField] private float _additionalDamage = 10f;
    private float _fianlAdditionalDamage = 0f;
    private float _explodeRange;

    protected override void Awake()
    {
        base.Awake();

        _additionalDamage = AccessoryManager.Instance.GetData(Type).GetStatBonusData(EStatType.BombDamage);
        _fianlAdditionalDamage = _additionalDamage;
        _explodeRange = AccessoryManager.Instance.GetData(Type).GetStatBonusData(EStatType.ExplodeRange);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (IsEqiupped)
        {
            return;
        }

        if (other.CompareTag("Player") && other.TryGetComponent<IItemReceiver>(out var receiver))
        {
            SetEquipped(true);
            receiver.ReceiveAccessory(Type, this);
        }
    }

    public void OnAttack()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _explodeRange, LayerMask.GetMask("Enemy"));
        if (hits.Length > 0)
        {
            //Instantiate(_bombEffectPrefab, transform.position, Quaternion.identity);
            PlayerEffectPoolManager.Instance.GetObject(EPlayerEffectType.BombEffect, transform.position, Quaternion.identity);
        }

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(_fianlAdditionalDamage);
                Debug.Log($"폭탄! {_fianlAdditionalDamage}");
            }
        }
    }

    public void OnEquip()
    {
        Debug.LogWarning("1" + gameObject.name);

        _fianlAdditionalDamage = _additionalDamage * AccessoryManager.Instance.GetAccessory(Type).Count;
        Debug.Log($"{AccessoryManager.Instance.GetAccessory(Type).Count}, {_fianlAdditionalDamage}");
    }

    public void OnHit(IDamageable target)
    {
    }

    private void OnDrawGizmosSelected()
    {
        // 폭발 반경을 노란색 반투명으로 시각화
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f); // 주황빛 반투명
        Gizmos.DrawSphere(transform.position, _explodeRange);

        // 테두리 강조
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explodeRange);
    }
}