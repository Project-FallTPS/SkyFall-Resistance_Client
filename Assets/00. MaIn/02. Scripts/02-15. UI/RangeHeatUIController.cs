using UnityEngine;
using UnityEngine.UI;

public class RangeHeatUIController : MonoBehaviour
{
    [SerializeField] private Image WingFill;

    private void OnEnable()
    {
        RangeHeatController.OnHeatChanged += UpdateHeatUI;
    }

    private void OnDisable()
    {
        RangeHeatController.OnHeatChanged -= UpdateHeatUI;
    }

    private void UpdateHeatUI(float currentHeat, float maxHeat, bool isOverheated)
    {
        float ratio = Mathf.Clamp01(currentHeat / maxHeat);

        WingFill.fillAmount = ratio;
    }
}
