using UnityEngine;

public class HavingPerkPanelUI : MonoBehaviour
{
    [SerializeField] private SlotUI[] _itemSlot;

    private void Start()
    {
        foreach(var slot in _itemSlot)
        {
            slot.Init();
        }
    }
}