using System.Collections.Generic;
using UnityEngine;

public class HavingPerkPanelUI : MonoBehaviour
{
    [Header("# Hierarchy")]
    [SerializeField] private Canvas _mainCanvas;

    [Header(" Project")]
    [SerializeField] private GameObject _slotPrefab;

    private List<SlotUI> _slots = new List<SlotUI>();

    private void Start()
    {
        SetHavingSlots();
    }

    private void SetHavingSlots() // 보유 판넬 초기화
    {
        foreach (var perk in PerkManager.Instance.HavingPerks)
        {
            for (int i = 0; i < perk.Value; i++)
            {
                GameObject newSlot = Instantiate(_slotPrefab, transform);
                SlotUI newSlotUI = newSlot.GetComponent<SlotUI>();
                newSlotUI.Init(PerkManager.Instance.PerkDatas[perk.Key], _mainCanvas);
                _slots.Add(newSlotUI);
            }
        }
        if (_slots.Count == 0)
        {
            GameObject newSlot = Instantiate(_slotPrefab, transform);
            SlotUI newSlotUI = newSlot.GetComponent<SlotUI>();
            newSlotUI.Init(null, _mainCanvas);
            _slots.Add(newSlotUI);
        }
    }
}