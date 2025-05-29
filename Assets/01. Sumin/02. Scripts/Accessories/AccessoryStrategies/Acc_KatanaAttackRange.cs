using UnityEngine;

public class Acc_KatanaAttackRange : AccessoryBase, IAccessory
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

    public override void SetEquipped(bool flag)
    {
        IsEqiupped = flag;
    }

    public void OnHit(IDamageable target, float baseDamage)
    {
    }
}