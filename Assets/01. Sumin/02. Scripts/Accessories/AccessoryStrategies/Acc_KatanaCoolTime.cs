using UnityEngine;

public class Acc_KatanaCoolTime : AccessoryBase, IAccessory
{
    public void OnEquip()
    {
        Animator anim = FindAnyObjectByType<KatanaEventHolder>().GetComponent<Animator>();
        if(anim != null)
        {
            anim.SetFloat("MeleeAnimationSpeed", 1f + Mathf.Log(AccessoryManager.Instance.GetAccessory(Type).Count + 1, 4));
        }
    }

    public void OnAttack()
    {
    }

    public void OnHit(IDamageable target)
    {
    }
}