using UnityEngine;
using System.Collections.Generic;

public class KatanaStrategy : IWeaponStrategy
{
    private WeaponData _weaponData;
    private PlayerAttackHandler _player;
    private Dictionary<EAccessoryType, AccessoryData> _equippedAccessories = new Dictionary<EAccessoryType, AccessoryData>();
    private Dictionary<EAccessoryType, Transform> _accessorySockets = new Dictionary<EAccessoryType, Transform>();

    [Header("# Target Dash")]
    private float _dashDuration = 0.1f;
    private float _dashTimer = 0f;
    private bool _isDashing = false;
    private Vector3 _dashStartPos;
    private Vector3 _dashTargetPos;

    public KatanaStrategy(PlayerAttackHandler player)
    {
        _weaponData = player.WeaponStat.GetWeaponData(EWeaponType.Katana);
        _player = player;
        InitializeAccessorySockets();
    }

    private void InitializeAccessorySockets()
    {
        // 무기 프리팹에서 소켓 Transform들을 찾아서 초기화
        Transform weaponTransform = _player.transform.Find("Katana");
        if (weaponTransform != null)
        {
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
    }

    public float GetDamage()
    {
        float baseDamage = _weaponData.Damage;
        float bonus = _player.PlayerStat.GetStat(EStatType.Damage);
        return baseDamage * bonus;
    }

    public float GetAttackSpeed()
    {
        float baseSpeed = _weaponData.CoolTime;
        float bonus = _player.PlayerStat.GetStat(EStatType.AttackSpeed);
        return baseSpeed * bonus;
    }

    public void AddAccessory(AccessoryData accessory)
    {
        if (!_accessorySockets.ContainsKey(accessory.Type))
        {
            return;
        }
        if (_equippedAccessories.ContainsKey(accessory.Type))
        {
            RemoveAccessory(accessory.Type);
        }
        _equippedAccessories[accessory.Type] = accessory;
        if (accessory.Prefab != null)
        {
            GameObject.Instantiate(accessory.Prefab, _accessorySockets[accessory.Type]);
        }

        if(accessory.Prefab.TryGetComponent<IAccesory>(out var acc))
        {
            acc.StatExecute(_weaponData);
        }
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

    public void Attack(IDamageable target)
    {
        if (target is MonoBehaviour mb)
        {
            StartDash(mb.gameObject);
        }
    }

    public void Attack(GameObject target)
    {
        StartDash(target);
    }

    public void StartDash(GameObject target)
    {
        if (_isDashing || target == null)
        {
            return;
        }
        if (!_player.PlayerStat.TryUseStamina(EStatType.TargetDashStaminaUseRate))
        {
            return;
        }

        _dashStartPos = _player.transform.position;
        _dashTargetPos = target.transform.position;

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
                _player.GetComponent<CharacterController>().Move(_dashTargetPos - _player.transform.position);
                _isDashing = false;
                Debug.Log(GetDamage());
                ExecuteAccesories();
            }
            else
            {
                Vector3 nextPos = Vector3.Lerp(_dashStartPos, _dashTargetPos, t);
                Vector3 moveDelta = nextPos - _player.transform.position;
                _player.GetComponent<CharacterController>().Move(moveDelta);
            }
        }
    }

    public void Update()
    {
        Dash();
    }

    public void ExecuteAccesories()
    {
        foreach(var acc in _equippedAccessories)
        {
            if(acc.Value.Prefab.TryGetComponent<IAccesory>(out var accesory))
            {
                accesory.Excecute();
            }
        }
    }
}
