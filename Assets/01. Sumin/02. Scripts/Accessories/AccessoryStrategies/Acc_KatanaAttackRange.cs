using UnityEngine;

public class Acc_KatanaAttackRange : AccessoryBase, IAccessory
{
    private float _scaleAmount = 1.1f;

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

    public override void SetEquipped(bool flag)
    {
        IsEqiupped = flag;
    }

    public void OnEquip()
    {
        int count = AccessoryManager.Instance.GetAccessory(Type).Count;

        transform.localScale = new Vector3(1, 1, Mathf.Pow(_scaleAmount, count));
    }

    public void OnAttack()
    {
    }

    public void OnHit(IDamageable target)
    {
    }
}