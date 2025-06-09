using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UI_Popup : MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.localScale = Vector3.zero;
    }

    public void Open()
    {
        gameObject.SetActive(true);
        _rectTransform.localScale = Vector3.zero;
        _rectTransform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack);
    }

    public void Close()
    {
        _rectTransform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

    // 🎯 새로운 콜백 버전 (외부 제어용)
    public void Open(System.Action onComplete)
    {
        gameObject.SetActive(true);
        _rectTransform.localScale = Vector3.zero;
        _rectTransform.DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            });
    }

    public void Close(System.Action onComplete)
    {
        _rectTransform.DOScale(Vector3.zero, 0.3f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
    }
}
