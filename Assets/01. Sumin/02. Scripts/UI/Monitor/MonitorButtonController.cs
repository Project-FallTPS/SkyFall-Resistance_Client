using UnityEngine;

public class MonitorButtonController : MonoBehaviour
{
    [Header("Hierarchy")]
    [SerializeField] private UI_Popup _perkPanel;
    [SerializeField] private UI_Popup _optionPanel;
    [SerializeField] private UI_Popup _exitPanel;

    public void OnClickStartButton()
    {
        // 게임 시작 메서드
    }
    public void OnClickPerkButton()
    {
        LobbySceneManager.Instance.OpenUI(_perkPanel);
    }

    public void OnClickOptionButton()
    {
        LobbySceneManager.Instance.OpenUI(_optionPanel);
    }

    public void OnClickExitButton()
    {
        LobbySceneManager.Instance.OpenUI(_exitPanel);
    }
}