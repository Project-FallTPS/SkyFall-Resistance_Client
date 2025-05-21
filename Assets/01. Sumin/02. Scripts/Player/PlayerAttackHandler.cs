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
    private Animator _animator;

    private Dictionary<EWeaponType, IWeaponStrategy> _strategies = new Dictionary<EWeaponType, IWeaponStrategy>();

    private EWeaponType _currentWeapon;
    private IWeaponStrategy _currentStrategy;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        PlayerStat = GetComponent<PlayerStatHolder>();
        _strategies.Add(EWeaponType.Katana, new KatanaStrategy(this));
        ChangeWeapon(EWeaponType.Katana);
    }

    private void Update()
    {
        _currentStrategy.Update();
    }

    public void ReceiveAccessory(EAccessoryType type, GameObject accessory)
    {
        _currentStrategy.AddAccessory(type, accessory);
    }

    public void ChangeWeapon(EWeaponType type)
    {
        if (_strategies.TryGetValue(type, out var strategy))
        {
            // TODO : Weapon 오브젝트 활성화

            _currentStrategy = strategy;
            _currentWeapon = type;

            Debug.Log($"무기 변경: {type}");

            switch(type)
            {
                case EWeaponType.Katana:
                    _animator.SetLayerWeight(1, 0f);
                    _animator.SetLayerWeight(2, 1f);
                    break;
                case EWeaponType.AR:
                    _animator.SetLayerWeight(1, 1f);
                    _animator.SetLayerWeight(2, 0f);
                    break;
            }    
        }
    }

    public void PerformAttack()
    {
        if (TargetManager.Instance.Target != null)
        {
            _currentStrategy.Attack(TargetManager.Instance.Target);

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