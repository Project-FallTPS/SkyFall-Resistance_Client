using UnityEngine;

public class Acc_KatanaBattery : AccessoryBase, IAccessory
{
    [SerializeField] private GameObject _electricEffectPrefab;
    [SerializeField] private float _additionalDamage = 10f;
    private float _fianlAdditionalDamage = 0f;

    protected override void Awake()
    {
        base.Awake();

        _additionalDamage = AccessoryManager.Instance.GetData(Type).GetStatBonusData(EStatType.BonusDamage);
        _fianlAdditionalDamage = _additionalDamage;
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
        // 공격 시작 시 호출되는 로직
    }

    public void OnEquip()
    {
        Debug.LogWarning("1" + gameObject.name);

        _fianlAdditionalDamage = _additionalDamage * AccessoryManager.Instance.GetData(Type).Count;
        Debug.Log($"{AccessoryManager.Instance.GetData(Type).Count}, {_fianlAdditionalDamage}");
    }

    public void OnHit(IDamageable target, float baseDamage)
    {
        Debug.LogWarning("2" + gameObject.name);
        Debug.Log($"OnHit Damage : {_fianlAdditionalDamage}");
        // 전기 데미지 추가
        target.TakeDamage(_fianlAdditionalDamage);
        
        // 전기 이펙트 생성S
        if (_electricEffectPrefab != null)
        {
            MonoBehaviour t = target as MonoBehaviour;
            Instantiate(_electricEffectPrefab, t.transform.position, Quaternion.identity);
        }
    }
}
