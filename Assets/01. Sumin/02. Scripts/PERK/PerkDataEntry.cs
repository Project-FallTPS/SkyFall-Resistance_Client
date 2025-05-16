using System.Collections.Generic;

[System.Serializable]
public class PerkDataEntry // 개별 퍽의 보너스 능력치
{
    public EPerkType Type;
    public List<PerkStatBonus> Bonuses;
}
