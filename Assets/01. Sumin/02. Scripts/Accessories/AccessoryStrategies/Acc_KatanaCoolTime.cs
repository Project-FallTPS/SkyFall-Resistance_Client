using UnityEngine;

public class Acc_KatanaCoolTime : AccessoryBase, IAccessory
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
    }

    public void OnAttack()
    {
    }

    public void OnHit(IDamageable target, float baseDamage)
    {
    }
}