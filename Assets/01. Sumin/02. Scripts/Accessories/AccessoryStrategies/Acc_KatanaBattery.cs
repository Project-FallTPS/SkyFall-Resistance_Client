using UnityEngine;

public class Acc_KatanaBattery : AccessoryBase, IAccessory
{
    [SerializeField] private GameObject _electricEffectPrefab;
    [SerializeField] private float _additionalDamage = 10f;
    private float _fianlAdditionalDamage = 0f;

    protected override void Awake()
    {
        base.Awake();

        _additionalDamage = AccessoryManager.Instance.GetData(Type).GetStatBonusData(EStatType.BatteryDamage);
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
    }

    public void OnEquip()
    {
        _fianlAdditionalDamage = _additionalDamage * AccessoryManager.Instance.GetAccessory(Type).Count;
    }

    public void OnHit(IDamageable target)
    {
        target.TakeDamage(_fianlAdditionalDamage);
        
        if (_electricEffectPrefab != null)
        {
            //TODO : 이펙트 풀
            MonoBehaviour t = target as MonoBehaviour;
            Instantiate(_electricEffectPrefab, t.transform.position, Quaternion.identity);
        }
    }
}
