using System;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("# WeaponData")]
    [SerializeField] private WeaponDataSO _weaponDatas;
    public WeaponDataSO WeaponDatas => _weaponDatas;

    private void Awake()
    {
        _weaponDatas.Init();
    }

    public WeaponData GetWeaponData(EWeaponType type)
    {
        return _weaponDatas.GetWeapon(type);
    }

    public bool TryShot(EWeaponType type)
    {
        if (GetWeaponData(type).CurrentAmmo > 0)
        {
            GetWeaponData(type).CurrentAmmo--;
            return true;
        }
        return false;
    }
}