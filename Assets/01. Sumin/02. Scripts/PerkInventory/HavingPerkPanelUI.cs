using UnityEngine;

public class HavingPerkPanelUI : MonoBehaviour
{
    [Header("# Hierarchy")]
    //[SerializeField] private SlotUI[] _itemSlot;
    [SerializeField] private Canvas _mainCanvas;

    [Header(" Project")]
    [SerializeField] private GameObject _slotPrefab;

    private void Start()
    {
        foreach(var perk in PerkManager.Instance.HavingPerks)
        {
            for (int i = 0; i < perk.Value; i++)
            {
                GameObject newSlot = Instantiate(_slotPrefab, transform);
                newSlot.GetComponent<SlotUI>().Init(PerkManager.Instance.PerkDatas[perk.Key], _mainCanvas);
            }
        }
    }
}