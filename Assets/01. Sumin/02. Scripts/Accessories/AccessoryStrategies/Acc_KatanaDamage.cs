using UnityEngine;

public class Acc_KatanaDamage : AccessoryBase, IAccessory
{
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

    public void OnEquip()
    {
        // 타격 시 발동하는 특수 효과가 있을 경우 여기에 구현
    }

    public void OnAttack()
    {
    }

    public void OnHit(IDamageable target, float baseDamage)
    {
    }
}