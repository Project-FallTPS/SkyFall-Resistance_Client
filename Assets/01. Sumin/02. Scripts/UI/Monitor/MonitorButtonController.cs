using UnityEngine;

public class MonitorButtonController : MonoBehaviour
{
    [Header("Hierarchy")]
    [SerializeField] private UI_Popup _perkPanel;

    public void OnClickPerkButton()
    {
        LobbySceneManager.Instance.OpenUI(_perkPanel);
    }
}