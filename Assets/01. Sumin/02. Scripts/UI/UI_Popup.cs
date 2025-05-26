using UnityEngine;
using DG.Tweening;

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
}
