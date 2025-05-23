using UnityEngine;

public class EquippPerkPanelUI : MonoBehaviour
{
    [Header("# Hierarchy")]
    [SerializeField] private SlotUI[] _itemSlot;

    private void Start() //ÀåÂø ÆÇ³Ú ÃÊ±âÈ­
    {
        foreach(var slot in _itemSlot)
        {
            slot.Clear();
        }
    }
}