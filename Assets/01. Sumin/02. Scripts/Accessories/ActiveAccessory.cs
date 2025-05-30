public class ActiveAccessory
{
    public AccessoryData Data;
    public IAccessory Object;
    public int Count;

    public ActiveAccessory(AccessoryData data, IAccessory obj)
    {
        Data = data;
        Object = obj;
        Count = 0;
    }
}