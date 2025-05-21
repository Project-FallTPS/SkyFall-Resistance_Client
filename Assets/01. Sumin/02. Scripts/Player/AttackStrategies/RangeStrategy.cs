using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class RangeStrategy : IWeaponStrategy
{
    private const string WEAPON_NAME = "Range";
    private WeaponData _weaponData;
    private PlayerAttackHandler _player;
    private Transform _muzzle;
    private Dictionary<EAccessoryType, AccessoryData> _equippedAccessories = new Dictionary<EAccessoryType, AccessoryData>();
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
        foreach (var data in _equippedAccessories)
        {
            var accessories = AccessoryManager.Instance.GetData(data.Key);
            accBonuses *= accessories.GetData(type);
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
                    obj.GetComponent<IBullet>().SetStats(GetStat(EStatType.Damage), GetStat(EStatType.MoveSpeed), dir);
                });
            _timer = 0f;
            _player.Anim.SetTrigger("anim_Player_Trigger_RangeAttack");
        }
    }

    public void Update()
    {
        _timer += Time.deltaTime;
    }

    public void AddAccessory(EAccessoryType type, GameObject obj)
    {
        if (!_accessorySockets.ContainsKey(type))
        {
            return;
        }
        if (_equippedAccessories.ContainsKey(type))
        {
            RemoveAccessory(type);
        }
        _equippedAccessories[type] = AccessoryManager.Instance.GetData(type);
        Debug.Log(type);
        obj.transform.SetParent(_accessorySockets[type]);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    public void RemoveAccessory(EAccessoryType type)
    {
        if (_equippedAccessories.ContainsKey(type))
        {
            _equippedAccessories.Remove(type);
            if (_accessorySockets.TryGetValue(type, out Transform socket))
            {
                foreach (Transform child in socket)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
    }

    public List<AccessoryData> GetEquippedAccessories()
    {
        return new List<AccessoryData>(_equippedAccessories.Values);
    }

    public void ExecuteAccesories()
    {
        foreach (var acc in _equippedAccessories)
        {
            if (acc.Value.Prefab.TryGetComponent<IAccessory>(out var accesory))
            {
                accesory.Excecute();
            }
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