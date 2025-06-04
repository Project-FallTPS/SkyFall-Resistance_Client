using UnityEngine;
using UnityEngine.UI;

public class UI_BossHealth : MonoBehaviour
{
    [SerializeField] private Slider _sliderPhase0;
    [SerializeField] private Slider _sliderPhase1;
    [SerializeField] private Slider _sliderPhase2;

    private void OnEnable()
    {
        BossController.OnBossHealthChange += UpdateHealthUI;
    }

    private void OnDisable()
    {
        BossController.OnBossHealthChange -= UpdateHealthUI;
    }

    private void UpdateHealthUI(float current, float max, int phase)
    {
        float ratio = current / max;

        _sliderPhase0.value = (phase == 0) ? ratio : 0f;
        _sliderPhase1.value = (phase == 1) ? ratio : (phase < 1 ? 1f : 0f);
        _sliderPhase2.value = (phase == 2) ? ratio : (phase < 2 ? 1f : 0f);
    }
}
