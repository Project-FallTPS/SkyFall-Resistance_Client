using UnityEngine;

public class MonitorButtonController : MonoBehaviour
{
    [Header("Hierarchy")]
    [SerializeField] private PopupUI _perkPanel;

    public void OnClickPerkButton()
    {
        LobbySceneManager.Instance.OpenUI(_perkPanel);
    }
}