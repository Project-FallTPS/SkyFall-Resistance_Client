using UnityEngine;
using UnityEngine.UI;

public class InGame1_UI_PlayerOutBoundaryAlert : MonoBehaviour
{
    public static InGame1_UI_PlayerOutBoundaryAlert instance;

    [SerializeField] private GameObject _alertUI;
    [SerializeField] private Text _timerText;
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

    private void OnDisable()
    {
        if (_playerArea != null)
            _playerArea.OnOutBoundaryTimerUpdate -= UpdateOutBoundaryTimerUI;
    }

    private void Start()
    {
        if (_alertUI == null) Debug.LogError("AlertUI Canvas is Not Assigned");
        else _alertUI.SetActive(false);

        if (_timerText != null)
            _timerText.text = "";
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
        float remaining = Mathf.Max(0f, _playerArea.PlayerZoneRadius - currentTime); // ¶Ç´Â KillTime - currentTime
        if (_timerText != null)
            _timerText.text = $"{remaining:F1}s";
    }
}
