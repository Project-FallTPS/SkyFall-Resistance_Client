using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGame1_UI_PlayerOutBoundaryAlert : MonoBehaviour
{
    public static InGame1_UI_PlayerOutBoundaryAlert instance;

    [SerializeField] private GameObject _alertUI;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private PlayerArea _playerArea;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void OnEnable()
    {
        if (_playerArea != null)
            _playerArea.OnOutBoundaryTimerUpdate += UpdateOutBoundaryTimerUI;
    }
    
    private void Start()
    {
        if (_alertUI != null)
        {
            _alertUI.SetActive(false);
        }

        if (_timerText != null)
        {
            _timerText.text = "";
        }
    }

    private void OnDisable()
    {
        if (_playerArea != null)
            _playerArea.OnOutBoundaryTimerUpdate -= UpdateOutBoundaryTimerUI;
    }

    public void OutBoundaryAlertOn()
    {
        _alertUI.SetActive(true);
        if (_timerText != null)
            _timerText.gameObject.SetActive(true);
    }

    public void OutBoundaryAlertOff()
    {
        _alertUI.SetActive(false);
        if (_timerText != null)
        {
            _timerText.text = "";
            _timerText.gameObject.SetActive(false);
        }
    }

    public void UpdateOutBoundaryTimerUI(float currentTime)
    {
        float remaining = Mathf.Max(0f, _playerArea.PlayerZoneRadius - currentTime); // �Ǵ� KillTime - currentTime
        if (_timerText != null)
            _timerText.text = $"{remaining:F1}s";
    }
}
