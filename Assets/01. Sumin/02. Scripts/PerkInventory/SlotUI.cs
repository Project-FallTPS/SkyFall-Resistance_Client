using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// Canvas -> Overlay로 하기
public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public static SlotUI DraggingSlot; // 드래그 중인 슬롯 (정적)

    [Header("# Hierarchy")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _bonusText;
    [SerializeField] private Image _background;
    [SerializeField] private Canvas _mainCanvas;

    [Header("# Project")]
    public bool IsEquipInventory = false;
    private Transform _originalParent;
    private Vector3 _originalPosition;

    private PerkDataEntry _data = null;

    public void Init(PerkDataEntry data, Canvas canvas)
    {
        _mainCanvas = canvas;
        _data = data;
        RefreshUI();
    }

    private string GetBonusText(PerkDataEntry data)
    {
        string result = "";
        foreach (var bonus in data.Bonuses)
        {
            result += $"{bonus.StatType}: x{bonus.Value}\n";
        }
        return result.TrimEnd();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerPress == _icon.gameObject || _data == null)
            return;

        DraggingSlot = this;

        // 원래 위치 저장
        _originalParent = _icon.transform.parent;
        _originalPosition = _icon.transform.position;

        // Canvas로 이동
        _icon.transform.SetParent(_mainCanvas.transform);
        _icon.raycastTarget = false; // 드래그 중에는 Raycast 막기
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (DraggingSlot == this)
        {
            _icon.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (DraggingSlot == this)
        {
            _icon.transform.SetParent(_originalParent);
            _icon.transform.position = _originalPosition;
            _icon.raycastTarget = true;

            RefreshUI();
            DraggingSlot = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var fromSlot = DraggingSlot;
        if (fromSlot != null && fromSlot != this)
        {
            SwapPerks(fromSlot);
        }
    }

    private void SwapPerks(SlotUI other)
    {
        if (_data == null && other._data == null) return;

        bool thisWasEquipped = IsEquipInventory && _data != null;
        bool otherWasEquipped = other.IsEquipInventory && other._data != null;

        var oldThisData = _data;
        var oldOtherData = other._data;

        // 데이터 교환
        (_data, other._data) = (other._data, _data);

        // UI 갱신
        RefreshUI();
        other.RefreshUI();

        // 장착 상태 갱신
        UpdateEquipStatus(thisWasEquipped, oldThisData, _data, IsEquipInventory);
        UpdateEquipStatus(otherWasEquipped, oldOtherData, other._data, other.IsEquipInventory);
    }

    private void UpdateEquipStatus(bool wasEquipped, PerkDataEntry oldData, PerkDataEntry newData, bool isEquipSlot)
    {
        if (!isEquipSlot) return;

        bool nowEquipped = newData != null;

        if (wasEquipped && !nowEquipped && oldData != null)
        {
            // 기존에 장착되어 있었지만, 이제 장착이 해제된 경우
            PerkManager.Instance.UnEquipPerk(oldData.Type);
        }
        else if (!wasEquipped && nowEquipped && newData != null)
        {
            // 기존에 장착 안 되어 있었는데, 새로 장착된 경우
            PerkManager.Instance.EquipPerk(newData.Type);
        }
        else if (wasEquipped && nowEquipped && oldData != null && newData != null && oldData.Type != newData.Type)
        {
            // 기존에 장착된 것과 다른 것으로 교체된 경우
            PerkManager.Instance.UnEquipPerk(oldData.Type);
            PerkManager.Instance.EquipPerk(newData.Type);
        }
    }

    private void RefreshUI()
    {
        if (_data != null)
        {
            _bonusText.text = GetBonusText(_data);
            _icon.sprite = _data.Icon;
            _icon.color = Color.white;
            _background.color = Color.green;
        }
        else
        {
            _bonusText.text = "Empty";
            _icon.sprite = null;
            _icon.color = new Color(0, 0, 0, 0); // 투명하게 처리
            _background.color = Color.white;
        }
    }

    public void Clear()
    {
        _data = null;
        RefreshUI();
    }
}