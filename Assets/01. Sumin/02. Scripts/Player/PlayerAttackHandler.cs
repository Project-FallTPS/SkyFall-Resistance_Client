using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackHandler : MonoBehaviour
{
    private Dictionary<EWeaponType, IWeaponStrategy> _strategies = new Dictionary<EWeaponType, IWeaponStrategy>();

    private EWeaponType _currentWeapon;
    private IWeaponStrategy _currentStrategy;

    private void Awake()
    {
        _strategies.Add(EWeaponType.Katana, new KatanaStrategy(gameObject));
        ChangeWeapon(EWeaponType.Katana);
    }

    private void Update()
    {
        _currentStrategy.Update();
    }

    public void ChangeWeapon(EWeaponType type)
    {
        if (_strategies.TryGetValue(type, out var strategy))
        {
            // TODO : Weapon 오브젝트 활성화

            _currentStrategy = strategy;
            _currentWeapon = type;

            Debug.Log($"무기 변경: {type}");
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