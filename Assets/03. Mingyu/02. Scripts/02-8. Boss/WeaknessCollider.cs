using System;
using UnityEngine;

public class WeaknessCollider : MonoBehaviour, IDamageable
{
    private BossController _bossController;
    private void Awake()
    {
        _bossController = GetComponentInParent<BossController>();
        Debug.Log(_bossController.gameObject.name);
    }

    public void TakeDamage(float damage)
    {
        _bossController.TakeDamage(damage * _bossController.BossData.WeaknessAttackDamageMultiplier);
    }
}
