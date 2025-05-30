using System.Collections.Generic;
using UnityEngine;

public class RangeStrategy : IWeaponStrategy
{
    private const string WEAPON_NAME = "Range";
    private WeaponData _weaponData;
    private PlayerAttackHandler _player;
    private Transform _muzzle;
    private Dictionary<EAccessoryType, Transform> _accessorySockets = new Dictionary<EAccessoryType, Transform>();

    private float _timer = 0f;

    public RangeStrategy(PlayerAttackHandler player)
    {
        _weaponData = WeaponDataManager.Instance.GetWeaponData(EWeaponType.Range);
        _player = player;
        InitializeAccessorySockets();
    }

    public void InitializeAccessorySockets()
    {
        Transform weaponTransform = null;
        foreach (var weapon in _player.Weapons)
        {
            if (weapon.name == WEAPON_NAME)
            {
                weaponTransform = weapon.transform;
                _muzzle = weaponTransform.Find("Muzzle");
                break;
            }
        }
        foreach (EAccessoryType type in System.Enum.GetValues(typeof(EAccessoryType)))
        {
            if (type == EAccessoryType.Count) continue;
            Transform socket = weaponTransform.Find($"Socket_{type}");
            if (socket != null)
            {
                _accessorySockets[type] = socket;
            }
        }
    }

    public float GetStat(EStatType type)
    {
        float baseDamage = _weaponData.GetStat(type);
        float perkBonus = PerkManager.Instance.EquippedPerkBonuses[type];
        float accBonuses = 1f;
        foreach (var data in AccessoryManager.Instance.GetEquippedAccessories(_weaponData.WeaponType))
        {
            accBonuses *= (1 + (data.Data.GetStatBonusData(type) - 1) * data.Count);
        }

        return baseDamage * perkBonus * accBonuses;
    }

    public void Attack(GameObject target)
    {
        if (_timer >= GetStat(EStatType.CoolTime))
        {
            Vector3 dir = SetDirection();
            Quaternion rot = Quaternion.LookRotation(dir);

            GameObject bullet = BulletPoolManager.Instance.GetObject(
                EBulletType.PlayerBullet,
                _muzzle.position,
                rot,
                (obj) =>
                {  
                    obj.GetComponent<IBullet>().SetStats(GetStat(EStatType.Damage), dir);
                });
            _timer = 0f;
            
            // 액세서리 효과 트리거
            AccessoryOnAttack();
        }
    }

    public void Update()
    {
        _timer += Time.deltaTime;
    }

    public void AddAccessory(EAccessoryType type, IAccessory newAccessory)
    {
        // 슬롯 존재 여부와 타입 유효성 검사
        if (!_accessorySockets.TryGetValue(type, out var socket) || !type.ToString().StartsWith(WEAPON_NAME))
            return;

        if (socket.childCount > 0)
        {
            AccessoryManager.Instance.Equip(type, newAccessory);
        }
        else
        {
            AccessoryManager.Instance.Equip(type, newAccessory);

            if (newAccessory is MonoBehaviour accessoryObj)
            {
                accessoryObj.transform.SetParent(socket);
                accessoryObj.transform.localPosition = Vector3.zero;
                accessoryObj.transform.localRotation = Quaternion.identity;

                if (accessoryObj.TryGetComponent(out AccessoryBase baseComponent))
                {
                    baseComponent.SetEquipped(true);
                }
            }
        }
    }

    public void RemoveAccessory(EAccessoryType type)
    {
        if (AccessoryManager.Instance.EquippedAccessories.ContainsKey(type))
        {
            AccessoryManager.Instance.UnEquip(type);
            if (_accessorySockets.TryGetValue(type, out Transform socket))
            {
                foreach (Transform child in socket)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
    }

    public void AccessoryOnAttack()
    {
        foreach (var acc in AccessoryManager.Instance.EquippedAccessories)
        {
            acc.Value.Object.OnAttack();
        }
    }

    private Vector3 SetDirection()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, ~((1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("AimCube")))))
        {
            return (hitInfo.point - _muzzle.position).normalized;
        }

        // Ray가 아무것도 맞지 않았을 경우: 카메라 기준 50f 앞 방향
        Vector3 fallbackPoint = ray.origin + ray.direction * 50f;
        return (fallbackPoint - _muzzle.position).normalized;
    }
}