using UnityEngine;
using System.Collections.Generic;

public class KatanaStrategy : IWeaponStrategy
{
    private const string WEAPON_NAME = "Katana";
    private WeaponData _weaponData;
    private PlayerAttackHandler _player;
    private Dictionary<EAccessoryType, AccessoryData> _equippedAccessories = new Dictionary<EAccessoryType, AccessoryData>();
    private Dictionary<EAccessoryType, Transform> _accessorySockets = new Dictionary<EAccessoryType, Transform>();

    [Header("# Target Dash")]
    private float _dashDuration = 0.15f;
    private float _dashTimer = 0f;
    private bool _isDashing = false;
    private Vector3 _dashStartPos;
    private Vector3 _dashTargetPos;
    private GameObject _target;

    private float _attackTimer = 100f;
    private float _targetDashTimer = 100f;
    private float _comboWindow = 1f;
    private float _lastAttackTime = 0f;
    private bool _isInComboWindow => Time.time - _lastAttackTime <= _comboWindow;

    public KatanaStrategy(PlayerAttackHandler player)
    {
        _weaponData = WeaponDataManager.Instance.GetWeaponData(EWeaponType.Katana);
        _player = player;
        InitializeAccessorySockets();
    }

    public void InitializeAccessorySockets()
    {
        Transform weaponTransform = null;
        foreach(var weapon in _player.Weapons)
        {
            if(weapon.name == WEAPON_NAME)
            {
                weaponTransform = weapon.transform;
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
        //if (_attackTimer >= GetStat(EStatType.CoolTime))
        //{
        //    if (Input.GetKey(KeyCode.LeftShift) && target != null)
        //    {
        //        StartDash(target);
        //    }
        //    else
        //    {
        //        //일반 공격
        //    }
        //}
        if (Input.GetKey(KeyCode.LeftShift) && target != null && _targetDashTimer >= GetStat(EStatType.CoolTime))
        {
            StartDash(target);
        }
        else if(_attackTimer >= GetStat(EStatType.CoolTime) || _isInComboWindow)
        {
            _lastAttackTime = Time.time;
            _player.Anim.SetTrigger("anim_Player_Trigger_MeleeAttack");
            _attackTimer = 0f;
        }
    }

    public void Update()
    {
        _attackTimer += Time.deltaTime;
        _targetDashTimer += Time.deltaTime;
        Dash();
    }

    public void StartDash(GameObject target)
    {
        if (_isDashing || target == null || !_player.PlayerStat.TryUseStamina(EStatType.TargetDashStaminaUseRate))
        {
            return;
        }
        _dashStartPos = _player.transform.position;
        _dashTargetPos = target.transform.position;
        _target = target;
        _dashTimer = 0f;
        _isDashing = true;
    }

    private void Dash()
    {
        if (_isDashing)
        {
            _dashTimer += Time.deltaTime;
            float t = _dashTimer / _dashDuration;

            if (t >= 1f)
            {
                _player.Rigid.MovePosition(_dashTargetPos);
                _isDashing = false;

                Debug.Log(GetStat(EStatType.Damage));

                if (_target != null && _target.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(GetStat(EStatType.Damage));
                }
                _targetDashTimer = 0f;
                _target = null;
                ExecuteAccesories();
            }
            else
            {
                Vector3 nextPos = Vector3.Lerp(_dashStartPos, _dashTargetPos, t);
                _player.Rigid.MovePosition(nextPos);
            }
        }
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
        foreach(var acc in _equippedAccessories)
        {
            if(acc.Value.Prefab.TryGetComponent<IAccessory>(out var accesory))
            {
                accesory.Excecute();
            }
        }
    }
}