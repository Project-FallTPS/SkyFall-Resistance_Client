using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour, IItemReceiver
{
    private const int MELEE_LAYER = 2;
    private const int RANGE_LAYER = 1;
    [Header("# Hierarchy")]
    [Header("# Weapon")]
    public List<GameObject> Weapons;

    [Header("# Stat")]
    public PlayerStatHolder PlayerStat { get; private set; }

    [Header("# Component")]
    public Animator Anim { get; private set; }
    public Rigidbody Rigid { get; private set; }

    private Dictionary<EWeaponType, IWeaponStrategy> _strategies = new Dictionary<EWeaponType, IWeaponStrategy>();

    private EWeaponType _currentWeapon;
    public IWeaponStrategy CurrentStrategy { get; private set; }

    private void Awake()
    {
        Rigid = GetComponentInChildren<Rigidbody>();
        Anim = GetComponentInChildren<Animator>();
        PlayerStat = GetComponent<PlayerStatHolder>();
        _strategies.Add(EWeaponType.Katana, new KatanaStrategy(this));
        _strategies.Add(EWeaponType.Range, new RangeStrategy(this));
    }

    private void Start()
    {
        ChangeWeapon(EWeaponType.Katana);
    }

    private void Update()
    {
        CurrentStrategy?.Update();
    }

    public void ReceiveAccessory(EAccessoryType type, IAccessory accessory)
    {
        if(type.ToString().StartsWith(nameof(EWeaponType.Range)))
        {
            _strategies[EWeaponType.Range].AddAccessory(type, accessory);
        }
        else if(type.ToString().StartsWith(nameof(EWeaponType.Katana)))
        {
            _strategies[EWeaponType.Katana].AddAccessory(type, accessory);
        }
        //CurrentStrategy?.AddAccessory(type, accessory);
    }

    public void ChangeWeapon(EWeaponType type)
    {
        UIEventHandler.Instance.OnPlayerWeaponChange?.Invoke(type);

        if (_strategies.TryGetValue(type, out var strategy))
        {
            CurrentStrategy = strategy;
            _currentWeapon = type;
            
            switch(type)
            {
                case EWeaponType.Katana:
                    Anim.SetLayerWeight(RANGE_LAYER, 0f); // Range
                    Anim.SetLayerWeight(MELEE_LAYER, 1f); // Melee
                    break;
                case EWeaponType.Range:
                    Anim.SetLayerWeight(RANGE_LAYER, 1f);
                    Anim.SetLayerWeight(MELEE_LAYER, 0f);
                    break;
            }

            foreach(var weapon in Weapons)
            {
                if(weapon.name == type.ToString())
                {
                    weapon.SetActive(true);
                }
                else
                {
                    weapon.SetActive(false);
                }
            }
        }
    }

    public void PerformAttack()
    {
        CurrentStrategy?.Attack(TargetManager.Instance.Target);
    }
}