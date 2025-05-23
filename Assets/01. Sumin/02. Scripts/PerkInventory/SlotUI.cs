using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _bonusText;
    [SerializeField] private Image _background;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private EPerkType _perkType;
    [SerializeField] private SlotUI _dragPreviewPrefab;
    private SlotUI _dragPreview;

    public bool IsEquipInventory = false;
    private bool _isEquipped;
    private bool _isFilled = false;
    private PerkDataEntry _data;

    private Transform _originalParent;

    //테스트용 메소드
    public void Init()
    {
        PerkDataEntry data = PerkManager.Instance.PerkDatas[_perkType];
        _data = data;
        _perkType = data.Type;
        //_icon.sprite = data.Icon;
        _bonusText.text = GetBonusText(data);
        _isFilled = true;
        RefreshUI();
    }

    public void Init(PerkDataEntry data)
    {
        _data = data;
        _perkType = data.Type;
        //_icon.sprite = data.Icon;
        _bonusText.text = GetBonusText(data);
        _isFilled = true;
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
        if (eventData.pointerPressRaycast.gameObject != _icon.gameObject || !_isFilled)
            return;

        // 드래그 프리뷰 생성
        _dragPreview = Instantiate(_dragPreviewPrefab, _mainCanvas.transform);
        _dragPreview.Init(_data); // 데이터 복사
        //_dragPreview.GetComponent<CanvasGroup>().blocksRaycasts = false; // 드래그 중 Raycast 막기
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragPreview != null)
        {
            _dragPreview.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_dragPreview != null)
            Destroy(_dragPreview.gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        var fromSlot = eventData.pointerDrag?.GetComponent<SlotUI>();
        if (fromSlot != null && fromSlot != this)
        {
            // 그냥 데이터만 스왑
            SwapPerks(fromSlot);
        }
    }

    private void SwapPerks(SlotUI other)
    {
        Debug.Log($"{gameObject.name} <-> {other.gameObject.name}");

        // 둘 다 비어 있으면 아무것도 안 함
        if (!_isFilled && !other._isFilled)
            return;

        // this가 비어 있고 other는 filled → other의 데이터를 this로 이동
        if (!_isFilled && other._isFilled)
        {
            _data = other._data;
            _isFilled = true;

            other.Clear();
        }
        // this는 filled, other는 비어 있음 → this의 데이터를 other로 이동
        else if (_isFilled && !other._isFilled)
        {
            other._data = _data;
            other._isFilled = true;

            Clear();
        }
        // 둘 다 filled → 스왑
        else
        {
            var tempData = _data;
            var tempFilled = _isFilled;

            _data = other._data;
            _isFilled = other._isFilled;

            other._data = tempData;
            other._isFilled = tempFilled;
        }

        // UI 갱신
        _bonusText.text = _isFilled ? GetBonusText(_data) : "Empty";
        other._bonusText.text = other._isFilled ? GetBonusText(other._data) : "Empty";

        RefreshUI();
        other.RefreshUI();
    }

    private void RefreshUI()
    {
        //_isEquipped = _isFilled && PerkManager.Instance.EquippedPerks.ContainsKey(_perkType);
        _background.color = _isFilled ? Color.green : Color.white;
    }

    public void Clear()
    {
        _data = null;
        _perkType = default;
        _bonusText.text = "Empty";
        _isFilled = false;
        RefreshUI();
    }
}
