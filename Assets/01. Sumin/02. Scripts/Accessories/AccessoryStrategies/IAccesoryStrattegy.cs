public interface IAccessory
{
    public EAccessoryType Type { get; }
    public AccessoryData Data { get; }
    public void SetEquipped(bool flag);
    public void Excecute(); // 추가 행동
}