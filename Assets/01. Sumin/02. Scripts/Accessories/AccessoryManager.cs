using UnityEngine;

public class AccessoryManager : Singleton<AccessoryManager>
{
    [SerializeField] private AccessoryDataSO _dataSO;

    public AccessoryData GetData(EAccessoryType type)
    {
        return _dataSO.GetData(type);
    }
}
