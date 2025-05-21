using UnityEngine;
using System.Collections.Generic;

public interface IWeaponStrategy
{
    //public float GetDamage();
    //public float GetAttackSpeed();
    public float GetDamage(EStatType type);
    public void Attack(IDamageable target);
    public void Attack(GameObject target); // 지울거
    public void Update();

    // 악세서리 관련 메서드
    public void InitializeAccessorySockets();
    public void AddAccessory(EAccessoryType type, GameObject obj);
    public void RemoveAccessory(EAccessoryType type);
    public List<AccessoryData> GetEquippedAccessories();
    public void ExecuteAccesories();
}