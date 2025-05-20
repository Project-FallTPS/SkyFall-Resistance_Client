public interface IAccessory
{
    public EAccessoryType Type { get; }
    public AccessoryData Data { get; }
    public void StatExecute(WeaponData data); // 추가 스탯
    public void Excecute(); // 추가 행동
}