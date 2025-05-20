using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AccessoryDataSO", menuName = "Scriptable Objects/AccessoryDataSO")]
public class AccessoryDataSO : ScriptableObject
{
    public List<AccessoryData> AccessoryDatas;
}
