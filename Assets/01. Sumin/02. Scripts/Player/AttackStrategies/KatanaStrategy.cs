public class KatanaStrategy : IWeaponStrategy
{
    private WeaponData _weaponData;

    public KatanaStrategy()
    {
        _weaponData = WeaponManager.Instance.GetWeaponData(EWeaponType.Katana);
    }

    public float GetDamage()
    {
        float baseDamage = _weaponData.Damage;
        float bonus = PlayerStatManager.Instance.GetStat(EStatType.Damage);
        return baseDamage * bonus;
    }

    public float GetAttackSpeed()
    {
        float baseSpeed = _weaponData.CoolTime;
        float bonus = PlayerStatManager.Instance.GetStat(EStatType.AttackSpeed);
        return baseSpeed * bonus;
    }

    public void Attack(IDamageable target)
    {

    }

    public void Update()
    {
        
    }
}