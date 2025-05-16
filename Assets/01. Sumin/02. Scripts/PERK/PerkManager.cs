using System.Collections.Generic;
using UnityEngine;

public class PerkManager : Singleton<PerkManager>
{
    [Header("# Project")]
    [SerializeField]
    private PerkDataCollectionSO _perkDataCollection;

    public Dictionary<EPerkType, PerkDataEntry> EquippedPerks { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        EquippedPerks = new Dictionary<EPerkType, PerkDataEntry>();
    }

    public void EquipPerk(EPerkType type)
    {
        EquippedPerks.Add(type, _perkDataCollection.GetPerkData(type));
    }

    public void UnEquipPerk(EPerkType type)
    {
        EquippedPerks.Remove(type);
    }

    public void CalculateFinalStats(Dictionary<EStatType, float> stats)
    {
        foreach (var perk in EquippedPerks)
        {
            foreach (var bonus in perk.Value.Bonuses)
            {
                EStatType statType = bonus.StatType;
                stats[statType] *= bonus.Value;
            }
        }
    }
}