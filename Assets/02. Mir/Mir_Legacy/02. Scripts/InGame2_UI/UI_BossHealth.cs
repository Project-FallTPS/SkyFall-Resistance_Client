using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 보스 체력바 ― 3 Phase × (Front·Back) Filled Image 방식
/// ▸ frontBar : 즉시 반응  ▸ backBar : 부드럽게 따라옴
/// </summary>
public class UI_BossHealth : MonoBehaviour
{
    [Header("Phase 0")]
    [SerializeField] private Image frontBarP0;
    [SerializeField] private Image backBarP0;

    [Header("Phase 1")]
    [SerializeField] private Image frontBarP1;
    [SerializeField] private Image backBarP1;

    [Header("Phase 2")]
    [SerializeField] private Image frontBarP2;
    [SerializeField] private Image backBarP2;

    [Header("Tween Options")]
    [SerializeField] private float delay = 0.05f;   // backBar 시작 지연
    [SerializeField] private float smooth = 0.35f;   // backBar 이동시간
    [SerializeField] private Ease ease = Ease.OutCubic;

    private void OnEnable() => BossController.OnBossHealthChange += UpdateHealthUI;
    private void OnDisable() => BossController.OnBossHealthChange -= UpdateHealthUI;

    /// <param name="current">현재 체력</param>
    /// <param name="max">최대 체력</param>
    /// <param name="phase">현재 페이즈 (0 ~ 2)</param>
    private void UpdateHealthUI(float current, float max, int phase)
    {
        float ratio = Mathf.Clamp01(current / max);

        UpdatePhaseBar(phase, ratio);
        FillClearedPhases(phase);   // 이미 깎인 페이즈 0 처리
        ResetUpcomingPhases(phase); // 아직 시작 안한 페이즈는 1 유지
    }

    // ──────────────────────────────────────────────────────
    #region 내부 메서드
    void UpdatePhaseBar(int phase, float ratio)
    {
        (Image front, Image back) = phase switch
        {
            0 => (frontBarP0, backBarP0),
            1 => (frontBarP1, backBarP1),
            2 => (frontBarP2, backBarP2),
            _ => (null, null)
        };

        if (front == null || back == null) return;

        // 앞바 : 즉시
        front.fillAmount = ratio;

        // 뒷바 : 트윈으로 부드럽게
        back.DOKill();
        back.DOFillAmount(ratio, smooth)
            .SetDelay(delay)
            .SetEase(ease);
    }

    void FillClearedPhases(int currentPhase)
    {
        if (currentPhase > 0)
        {
            frontBarP0.fillAmount = backBarP0.fillAmount = 0f;
            if (currentPhase > 1)
                frontBarP1.fillAmount = backBarP1.fillAmount = 0f;
        }
    }

    void ResetUpcomingPhases(int currentPhase)
    {
        if (currentPhase < 2)
            frontBarP2.fillAmount = backBarP2.fillAmount = 1f;
        if (currentPhase < 1)
            frontBarP1.fillAmount = backBarP1.fillAmount = 1f;
    }
    #endregion
}

