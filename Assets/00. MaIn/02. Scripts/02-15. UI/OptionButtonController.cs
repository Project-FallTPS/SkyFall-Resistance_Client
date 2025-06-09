using UnityEngine;

public class OptionButtonController : MonoBehaviour
{
    [Header("Hierarchy")]
    [SerializeField] private UI_Popup _exitPanel;

    public void OnClickExitButton()
    {
        if (_exitPanel != null)
        {
            LobbySceneManager.Instance.OpenUI(_exitPanel);
        }
        else
        {
            Debug.LogWarning("Exit Panel is not assigned.");
        }
    }
}
