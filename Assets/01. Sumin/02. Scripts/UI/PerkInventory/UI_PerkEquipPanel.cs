using System.Collections.Generic;
using UnityEngine;

public class UI_PerkEquipPanel : UI_Popup
{
    [Header("# Hierarchy")]
    [SerializeField] private Canvas _mainCanvas;

    [Header("# Prefab")]
    [SerializeField] private UI_Slot[] _itemSlot;
    public UI_Slot[] ItemSlot => _itemSlot;
    [SerializeField] private GameObject _havingPanel;
    [SerializeField] private GameObject _slotPrefab;

    private List<UI_Slot> _havingSlots = new List<UI_Slot>();
    public List<UI_Slot> HavingSlots => _havingSlots;

    private void Start() //장착 판넬 초기화
    {
        // 장착된 퍽들 가져와서
        // slot들에 하나씩 장착하기
        foreach(var slot in _itemSlot)
        {
            slot.Init(null, _mainCanvas, this);
        }

        foreach (var perkData in PerkManager.Instance.EquippedPerks)
        {
            foreach(PerkDataEntry perk in perkData.Value)
            {
                foreach (UI_Slot slot in _itemSlot)
                {
                    if(slot.Data == null)
                    {
                        slot.Init(perk, _mainCanvas, this);
                        break;
                    }
                }
            }
        }
        SetHavingSlots();
    }

    private void SetHavingSlots() // 보유 판넬 초기화
    {
        foreach (var perk in PerkManager.Instance.HavingPerks)
        {
            for (int i = 0; i < perk.Value; i++)
            {
                GameObject newSlot = Instantiate(_slotPrefab, _havingPanel.transform);
                UI_Slot newSlotUI = newSlot.GetComponent<UI_Slot>();
                newSlotUI.Init(PerkManager.Instance.PerkDatas[perk.Key], _mainCanvas, this);
                _havingSlots.Add(newSlotUI);
            }
        }
        //if (_havingSlots.Count == 0)
        //{
        //    GameObject newSlot = Instantiate(_slotPrefab, _havingPanel.transform);
        //    SlotUI newSlotUI = newSlot.GetComponent<SlotUI>();
        //    newSlotUI.Init(null, _mainCanvas, this);
        //    _havingSlots.Add(newSlotUI);
        //}
    }
}