using UnityEngine;

public interface IWeaponStrategy
{
    public float GetDamage();
    public float GetAttackSpeed();
    public void Attack(IDamageable target); // �߻�
    public void Attack(GameObject target);
    public void Update();
}
