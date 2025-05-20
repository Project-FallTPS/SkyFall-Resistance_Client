using UnityEngine;
using System.Collections.Generic;

public interface IWeaponStrategy
{
    public float GetDamage();
    public float GetAttackSpeed();
    public void Attack(IDamageable target);
    public void Attack(GameObject target);
    public void Update();
    
    // 악세서리 관련 메서드 추가
    public void AddAccessory(AccessoryData accessory);
    public void RemoveAccessory(EAccessoryType type);
    public List<AccessoryData> GetEquippedAccessories();
    //public float GetAccessoryMultiplier(EAccessoryType type);
    public void ExecuteAccesories();
}
