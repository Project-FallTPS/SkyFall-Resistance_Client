public interface IAccessory
{
    public EAccessoryType Type { get; }
    public AccessoryData Data { get; }
    public void StatExecute(WeaponData data); // �߰� ����
    public void Excecute(); // �߰� �ൿ
}