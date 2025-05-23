using UnityEngine;
using System.Collections.Generic;

public interface IWeaponStrategy
{
    //public float GetDamage();
    //public float GetAttackSpeed();
    public float GetStat(EStatType type);
    public void Attack(GameObject target);
    public void Update();

    // 악세서리 관련 메서드
    public void InitializeAccessorySockets();
    public void AddAccessory(EAccessoryType type, GameObject obj);
    public void RemoveAccessory(EAccessoryType type);
    public List<AccessoryData> GetEquippedAccessories();
    public void ExecuteAccesories();
}