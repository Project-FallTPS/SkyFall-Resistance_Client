public interface IAccessory
{
    public EAccessoryType Type { get; }
    public AccessoryData Data { get; }
    public void Excecute(); // �߰� �ൿ
}