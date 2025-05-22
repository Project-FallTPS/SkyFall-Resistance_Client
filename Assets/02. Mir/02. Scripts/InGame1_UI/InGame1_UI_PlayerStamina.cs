using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InGame1_UI_PlayerStamina : MonoBehaviour
{
    //test
    [SerializeField] private Slider _staminaSlider;

    private void Awake()
    {
        if (_staminaSlider == null) Debug.LogError("Stamina Slider is Not Assigned");
    }

    private void OnEnable()
    {
        PlayerStatHolder.OnStaminaChange += UpdateStaminaUI;
    }

    private void OnDisable()
    {
        PlayerStatHolder.OnStaminaChange -= UpdateStaminaUI;
    }

    private void UpdateStaminaUI(float current, float max)
    {
        float normalizedValue = Mathf.Clamp01(current / max);
        _staminaSlider.value = normalizedValue;
    }
}
