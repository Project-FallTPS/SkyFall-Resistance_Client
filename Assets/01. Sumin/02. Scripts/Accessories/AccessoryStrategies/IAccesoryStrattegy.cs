public interface IAccessory
{
    public EAccessoryType Type { get; }
    public AccessoryData Data { get; }
    //public void SetEquipped(bool flag);
    public void OnEquip(); // 추가 행동
    public void OnAttack();
}