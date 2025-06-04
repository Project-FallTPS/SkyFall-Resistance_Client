using UnityEngine;

public class Acc_KatanaBattery : AccessoryBase, IAccessory
{
    [SerializeField] private float _additionalDamage = 10f;
    private float _fianlAdditionalDamage = 0f;

    protected override void Awake()
    {
        base.Awake();

        _additionalDamage = AccessoryManager.Instance.GetData(Type).GetStatBonusData(EStatType.BatteryDamage);
        _fianlAdditionalDamage = _additionalDamage;
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

        MonoBehaviour t = target as MonoBehaviour;
        PlayerEffectPoolManager.Instance.GetObject(EPlayerEffectType.BatteryEffect, t.transform.position, Quaternion.identity);
        UIEventHandler.Instance.OnPlayerAttackHit?.Invoke();
    }
}
