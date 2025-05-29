using UnityEngine;

public class Acc_KatanaAttackRange : AccessoryBase, IAccessory
{
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