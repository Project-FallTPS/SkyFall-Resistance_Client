using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PerkDataCollectionSO", menuName = "Scriptable Objects/PerkDataCollectionSO")]
public class PerkDataCollectionSO : ScriptableObject
{
    public List<PerkDataEntry> PerkDatas;

    private Dictionary<EPerkType, PerkDataEntry> _perkDataDictionary;

    public void Init()
    {
        _perkDataDictionary = new Dictionary<EPerkType, PerkDataEntry>();

        foreach(var p in PerkDatas)
        {
            _perkDataDictionary.Add(p.Type, p);
        }
    }

    public PerkDataEntry GetPerkData(EPerkType type)
    {
        if(_perkDataDictionary == null)
        {
            Init();
        }

        return _perkDataDictionary[type];
    }
}