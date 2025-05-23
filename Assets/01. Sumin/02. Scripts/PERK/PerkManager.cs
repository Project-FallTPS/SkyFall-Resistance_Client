using System.Collections.Generic;
using UnityEngine;

public class PerkManager : Singleton<PerkManager>
{
    private const string PERK_SAVE_DATA = "PerkSaveData";

    [Header("# Project")]
    [SerializeField] private PerkDataCollectionSO _perkDataCollection; // 베이스 데이터

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
        PerkSaveData data = null;
        HavingPerks = new Dictionary<EPerkType, int>();
        if(JsonDataManager.FileExists(PERK_SAVE_DATA))
        {
            data = JsonDataManager.LoadFromFile<PerkSaveData>(PERK_SAVE_DATA);
        }
        else //없으면 새로 만들기 -> default로 만들면 null참조가 일어나기 때문에 인스턴스를 직접 만들어서 할당
        {
            data = new PerkSaveData();
            foreach (EPerkType perkType in (EPerkType[])System.Enum.GetValues(typeof(EPerkType)))
            {
                data.OwnedPerks.Add(new PerkSaveEntry(perkType, 0));
            }
            JsonDataManager.CreateFile<PerkSaveData>(PERK_SAVE_DATA, data);
        }
        foreach (var entry in data.OwnedPerks)
        {
            HavingPerks.Add(entry.PerkType, entry.Count);
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

    public void InitPerkBonuses() // 장착된 퍽 기반 스탯보너스 계산
    {
        EquippedPerkBonuses.Clear();

        foreach (EStatType statType in (EPerkType[])System.Enum.GetValues(typeof(EStatType)))
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
}