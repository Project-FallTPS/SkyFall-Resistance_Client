using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class InGame1_UI_PlayerAltitude : MonoBehaviour
{
    [SerializeField] 
    private Slider _sliderAltitude;

    private void Start()
    {
        StartCoroutine(UpdateAltitudeSliderOverTime(WaveManager.Instance.FallSceneDuration));
    }

    private IEnumerator UpdateAltitudeSliderOverTime(float duration)
    {
        float elapsed = 0f;
        _sliderAltitude.value = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _sliderAltitude.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        _sliderAltitude.value = 1f;
    }
}
