using System;
using UnityEngine;

public class WeaknessCollider : MonoBehaviour, IDamageable
{
    private BossController _bossController;
    public Action<float> OnCriticalHit;
    
    private void Awake()
    {
        _bossController = GetComponentInParent<BossController>();
    }   
    public GameObject GameObject => gameObject;
    public void TakeDamage(float damage)
    {
        float criticalDamage = damage * _bossController.BossData.WeaknessAttackDamageMultiplier;
        _bossController.BossData.CurrentHealth -= criticalDamage;
        if (_bossController.BossData.CurrentHealth <= 0)
        {
            Debug.Log("Boss Dead");
        }
        else
        {
            Debug.Log("Boss Damaged");
        }
        
        OnCriticalHit?.Invoke(criticalDamage);
    }
}
