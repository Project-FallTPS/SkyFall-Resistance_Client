using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PerkDataEntry // ���� ���� ���ʽ� �ɷ�ġ
{
    public EPerkType Type;
    public Sprite Icon;
    public List<StatBonus> Bonuses;
}