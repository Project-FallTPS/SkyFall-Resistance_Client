using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackHandler : MonoBehaviour
{
    private Dictionary<EWeaponType, IWeaponStrategy> _strategies = new Dictionary<EWeaponType, IWeaponStrategy>();

    private EWeaponType _currentWeapon;
    private IWeaponStrategy _currentStrategy;

    private IDamageable _target;

    private void Awake()
    {
        _strategies.Add(EWeaponType.Katana, new KatanaStrategy());
    }

    private void Update()
    {

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

    public void PerformAttack()
    {
        //if (TargetManager.Instance.Target != null)
        //{
        //    if (TargetManager.Instance.Target.TryGetComponent<IDamageable>(out var t))
        //    {
        //        _currentStrategy.Attack(t);
        //    }
        //}
        //else
        //{

        //}
    }
}