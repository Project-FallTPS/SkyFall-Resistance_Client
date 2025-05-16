using UnityEngine;

public interface IWeaponStrategy
{
    public float GetDamage();
    public float GetAttackSpeed();
    public void Attack(IDamageable target); // 발사
    public void Update(); // Update문에서 실행할 것
}
