using UnityEngine;

public class EquippPerkPanelUI : MonoBehaviour
{
    [SerializeField] private SlotUI[] _itemSlot;

    private void Start()
    {
        foreach(var slot in _itemSlot)
        {
            slot.Clear();
        }
    }
}
