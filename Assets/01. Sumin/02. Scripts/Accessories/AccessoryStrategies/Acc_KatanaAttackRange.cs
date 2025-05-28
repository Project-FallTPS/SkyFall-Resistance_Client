using UnityEngine;

public class Acc_KatanaAttackRange : AccessoryBase, IAccessory
{
    public void Execute()
    {
        
    }

    public override void SetEquipped(bool flag)
    {
        IsEqiupped = flag;
    }
}