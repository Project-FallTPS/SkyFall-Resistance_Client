using System.Collections.Generic;
using UnityEngine;

public class PerkEquipPanelUI : MonoBehaviour
{
    [Header("# Hierarchy")]
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private SlotUI[] _itemSlot;
    public SlotUI[] ItemSlot => _itemSlot;
    [SerializeField] private GameObject _havingPanel;
    [SerializeField] private GameObject _slotPrefab;

    private List<SlotUI> _havingSlots = new List<SlotUI>();
    public List<SlotUI> HavingSlots => _havingSlots;

    private void Start() //장착 판넬 초기화
    {
        foreach(var slot in _itemSlot)
        {
            slot.Init(null, _mainCanvas, this);
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
                SlotUI newSlotUI = newSlot.GetComponent<SlotUI>();
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