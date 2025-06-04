using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerHealthBarController : MonoBehaviour
{
    [SerializeField] private Image frontBar;  // 밝은 빨간색 (즉시 반응)
    [SerializeField] private Image backBar;   // 어두운 빨간색 (느리게 따라감)
    [SerializeField] private float delay = 0.05f;
    [SerializeField] private float smoothTime = 0.35f;

    private float _currentRatio = 1f;

    private void OnEnable()
    {
        UIEventHandler.Instance.OnHealthChange += UpdateHealthBar;
    }

    private void OnDisable()
    {
        UIEventHandler.Instance.OnHealthChange -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float current, float max)
    {
        float ratio = Mathf.Clamp01(current / max);
        _currentRatio = ratio;

        // 앞바는 즉시
        frontBar.fillAmount = ratio;

        // 뒷바는 느리게 따라감
        backBar.DOKill();
        backBar.DOFillAmount(ratio, smoothTime)
               .SetDelay(delay)
               .SetEase(Ease.OutCubic);
    }
}
