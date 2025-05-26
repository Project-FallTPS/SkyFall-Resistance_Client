using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

// Canvas -> Overlay로 하기
public class UI_Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public static UI_Slot DraggingSlot; // 드래그 중인 슬롯 (정적)

    [Header("# Hierarchy")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _bonusText;
    [SerializeField] private Image _background;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private UI_PerkEquipPanel _panel;

    [Header("# Project")]
    public bool IsEquipInventory = false;
    private Transform _originalParent;
    private Vector3 _originalPosition;

    public PerkDataEntry Data { get; private set; }

    public void Init(PerkDataEntry data, Canvas canvas, UI_PerkEquipPanel panel)
    {
        _mainCanvas = canvas;
        _panel = panel;
        Data = data;
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(IsEquipInventory)
            {
                foreach(var slot in _panel.HavingSlots)
                {
                    if(slot.Data == null)
                    {
                        SwapPerks(slot);
                        break;
                    }
                }
            }
            else
            {
                foreach(var slot in _panel.ItemSlot)
                {
                    if(slot.Data == null)
                    {
                        SwapPerks(slot);
                        break;
                    }
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerPress == _icon.gameObject || Data == null)
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

    private void SwapPerks(UI_Slot other)
    {
        if (Data == null && other.Data == null) return;

        bool thisWasEquipped = IsEquipInventory && Data != null;
        bool otherWasEquipped = other.IsEquipInventory && other.Data != null;

        var oldThisData = Data;
        var oldOtherData = other.Data;

        // 데이터 교환
        (Data, other.Data) = (other.Data, Data);

        // UI 갱신
        RefreshUI();
        other.RefreshUI();

        // 장착 상태 갱신
        UpdateEquipStatus(thisWasEquipped, oldThisData, Data, IsEquipInventory);
        UpdateEquipStatus(otherWasEquipped, oldOtherData, other.Data, other.IsEquipInventory);
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
        if (Data != null)
        {
            _bonusText.text = GetBonusText(Data);
            _icon.sprite = Data.Icon;
            _icon.color = Color.white;
            _background.color = Color.green;
        }
        else
        {
            _bonusText.text = "";
            _icon.sprite = null;
            _icon.color = new Color(0, 0, 0, 0); // 투명하게 처리
            _background.color = Color.white;
        }
    }

    public void Clear()
    {
        Data = null;
        RefreshUI();
    }
}