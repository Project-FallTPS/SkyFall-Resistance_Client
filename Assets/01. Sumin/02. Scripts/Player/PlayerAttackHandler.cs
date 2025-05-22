using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackHandler : MonoBehaviour, IItemReceiver
{
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
        ChangeWeapon(EWeaponType.Range);
    }

    private void Update()
    {
        CurrentStrategy.Update();
    }

    public void ReceiveAccessory(EAccessoryType type, GameObject accessory)
    {
        CurrentStrategy.AddAccessory(type, accessory);
    }

    public void ChangeWeapon(EWeaponType type)
    {
        if (_strategies.TryGetValue(type, out var strategy))
        {
            // TODO : Weapon 오브젝트 활성화

            CurrentStrategy = strategy;
            _currentWeapon = type;

            Debug.Log($"무기 변경: {type}");

            switch(type)
            {
                case EWeaponType.Katana:
                    Anim.SetLayerWeight(1, 0f);
                    Anim.SetLayerWeight(2, 1f);
                    break;
                case EWeaponType.Range:
                    Anim.SetLayerWeight(1, 1f);
                    Anim.SetLayerWeight(2, 0f);
                    break;
            }    
        }
    }

    public void PerformAttack()
    {
        CurrentStrategy.Attack(TargetManager.Instance.Target);

        if (TargetManager.Instance.Target != null)
        {

            //if (TargetManager.Instance.Target.TryGetComponent<IDamageable>(out var t))
            //{
            //    //_currentStrategy.Attack(t);
            //}
        }
        else
        {

        }
    }

}