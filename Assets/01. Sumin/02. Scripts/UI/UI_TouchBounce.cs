using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class UI_TouchBounce : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float EndScale = 0.9f;
    public float StartScale = 1f;
    public float Duration = 0.2f;

    private RectTransform _rectTransform;
    private Button _button;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        StartScale = transform.localScale.x;
        _button = GetComponent<Button>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!_button.interactable)
        {
            return;
        }
        transform.DOScale(EndScale, Duration).SetEase(Ease.InOutBounce).OnComplete(() => transform.localScale = Vector3.one * EndScale).SetUpdate(true);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (!_button.interactable)
        {
            return;
        }
        transform.DOScale(StartScale, Duration).SetEase(Ease.InOutBounce).OnComplete(() => transform.localScale = Vector3.one * StartScale).SetUpdate(true);
    }

    public void Bounce()
    {
        if (!_button.interactable)
        {
            return;
        }
        transform.DOKill(); // 이전 트윈 제거
        transform.localScale = Vector3.one * StartScale;
        transform.DOScale(EndScale, Duration)
            .SetEase(Ease.InOutBounce)
            .OnComplete(() => transform.DOScale(StartScale, Duration).SetEase(Ease.InOutBounce));
    }
}