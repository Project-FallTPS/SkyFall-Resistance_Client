using UnityEngine;

public interface IWeaponStrategy
{
    public float GetDamage();
    public float GetAttackSpeed();
    public void Attack(IDamageable target); // น฿ป็
    public void Attack(GameObject target);
    public void Update();
}
