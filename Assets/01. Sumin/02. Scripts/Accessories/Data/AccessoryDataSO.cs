using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AccessoryDataSO", menuName = "Scriptable Objects/AccessoryDataSO")]
public class AccessoryDataSO : ScriptableObject
{
    public List<AccessoryData> AccessoryDatas;

    private Dictionary<EAccessoryType, AccessoryData> _dict;

    public void Init()
    {
        _dict = new Dictionary<EAccessoryType, AccessoryData>();
        foreach (var data in AccessoryDatas)
        {
            _dict[data.Type] = data;
        }
    }

    public AccessoryData GetData(EAccessoryType type)
    {
        if (_dict == null) Init();
        return _dict.TryGetValue(type, out var data) ? data : null;
    }
}
