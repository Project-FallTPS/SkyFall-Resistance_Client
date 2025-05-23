using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("# Hierarchy")]
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _bonusText;
    [SerializeField] private Image _background;
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private EPerkType _perkType;

    [Header("# Project")]
    [SerializeField] private SlotUI _dragPreviewPrefab;
    private SlotUI _dragPreview;

    public bool IsEquipInventory = false;
    private bool _isFilled = false;
    private PerkDataEntry _data = null;

    //테스트용 메소드
    public void Init()
    {
        if(_perkType == EPerkType.Count)
        {
            RefreshUI();
            return;
        }
        PerkDataEntry data = PerkManager.Instance.PerkDatas[_perkType];
        _data = data;
        _perkType = data.Type;
        _isFilled = true;
        RefreshUI();
    }

    public void Init(PerkDataEntry data)
    {
        _data = data;
        _perkType = data.Type;
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

    private void SwapPerks(SlotUI other) // other가 받는 부분
    {
        Debug.Log($"{_perkType} <-> {other._perkType}");

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
        if (_isFilled && _data != null)
        {
            _bonusText.text = GetBonusText(_data);
            _icon.sprite = _data.Icon; // Sprite가 없으면 여기서 null로 보일 수도 있음
            _icon.color = Color.white; // 안 보이는 문제 방지
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
        _perkType = default;
        _bonusText.text = "Empty";
        _isFilled = false;
        RefreshUI();
    }
}
