using UnityEngine;

public class EquippPerkPanelUI : MonoBehaviour
{
    [Header("# Hierarchy")]
    [SerializeField] private SlotUI[] _itemSlot;

    private void Start() //���� �ǳ� �ʱ�ȭ
    {
        foreach(var slot in _itemSlot)
        {
            slot.Clear();
        }
    }
}