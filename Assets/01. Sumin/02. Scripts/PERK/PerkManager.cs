using System.Collections.Generic;
using UnityEngine;

public class PerkManager : Singleton<PerkManager>
{
    private const string PERK_SAVE_DATA = "PerkSaveData";

    [Header("# Project")]
    [SerializeField]
    private PerkDataCollectionSO _perkDataCollection;

    public Dictionary<EPerkType, PerkDataEntry> PerkDatas { get; private set; } //모든 퍽 데이터
    public Dictionary<EPerkType, int> HavingPerks { get; private set; } //보유한 퍽
    public Dictionary<EPerkType, List<PerkDataEntry>> EquippedPerks { get; private set; } // 장착한 퍽
    public Dictionary<EStatType, float> EquippedPerkBonuses { get; private set; } //장착한 퍽 스탯 보너스 합산

    protected override void Awake()
    {
        base.Awake();
        EquippedPerks = new Dictionary<EPerkType, List<PerkDataEntry>>();
        EquippedPerkBonuses = new Dictionary<EStatType, float>();
        InitPerkBonuses();
        PerkDatas = _perkDataCollection.MakeDictionary();
        InitHavingPerks();
    }

    private void InitHavingPerks()
    {
        HavingPerks = new Dictionary<EPerkType, int>();
        PerkSaveData datas = JsonDataManager.LoadFromFile<PerkSaveData>(PERK_SAVE_DATA);
        foreach(var data in datas.OwnedPerks)
        {
            //Dic
            HavingPerks.Add(data.PerkType, data.Count);
        }
    }

    public void EquipPerk(EPerkType type)
    {
        var perkData = _perkDataCollection.GetPerkData(type);
        Debug.Log($"Equip {type}");
        if (!EquippedPerks.ContainsKey(type))
        {
            EquippedPerks[type] = new List<PerkDataEntry>();
        }

        EquippedPerks[type].Add(perkData);

        InitPerkBonuses();
    }

    public void UnEquipPerk(EPerkType type)
    {
        if (EquippedPerks.ContainsKey(type) && EquippedPerks[type].Count > 0)
        {
            Debug.Log($"UnEquip {type}");
            EquippedPerks[type].RemoveAt(0);

            // 리스트가 비면 아예 제거
            if (EquippedPerks[type].Count == 0)
            {
                EquippedPerks.Remove(type);
            }

            InitPerkBonuses();
        }
    }

    public void InitPerkBonuses()
    {
        EquippedPerkBonuses.Clear();

        foreach (EStatType statType in System.Enum.GetValues(typeof(EStatType)))
        {
            if (statType == EStatType.Count) continue;
            EquippedPerkBonuses[statType] = 1f;
        }

        foreach (var perkList in EquippedPerks.Values)
        {
            foreach (var perk in perkList)
            {
                foreach (var bonus in perk.Bonuses)
                {
                    EquippedPerkBonuses[bonus.StatType] *= bonus.Value;
                }
            }
        }
    }

    public bool IsEquipped(EPerkType type)
    {
        return EquippedPerks.ContainsKey(type) && EquippedPerks[type].Count > 0;
    }
}