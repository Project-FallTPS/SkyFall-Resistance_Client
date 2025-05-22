using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PerkDataEntry // 개별 퍽의 보너스 능력치
{
    public EPerkType Type;
    public Sprite Icon;
    public List<StatBonus> Bonuses;
}