public interface IAccessory
{
    public EAccessoryType Type { get; }
    public AccessoryData Data { get; }
    //public void SetEquipped(bool flag);
    public void OnEquip(); // 장착 시 호출
    public void OnAttack(); // 공격 시 호출
    public void OnHit(IDamageable target, float baseDamage); // 타격 시 호출
}