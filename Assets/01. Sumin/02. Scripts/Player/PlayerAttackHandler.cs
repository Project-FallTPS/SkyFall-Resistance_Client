using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackHandler : MonoBehaviour
{
    private Dictionary<EWeaponType, IWeaponStrategy> _strategies = new Dictionary<EWeaponType, IWeaponStrategy>();

    private EWeaponType _currentWeapon;
    private IWeaponStrategy _currentStrategy;

    private void Awake()
    {
        _strategies.Add(EWeaponType.Katana, new KatanaStrategy());
    }

    public void ChangeWeapon(EWeaponType type)
    {
        if (_strategies.TryGetValue(type, out var strategy))
        {
            // TODO : Weapon ������Ʈ Ȱ��ȭ

            _currentStrategy = strategy;
            _currentWeapon = type;

            Debug.Log($"���� ����: {type}");
        }
    }

    public void PerformAttack(IDamageable target)
    {
        _currentStrategy.Attack(target);
    }
}