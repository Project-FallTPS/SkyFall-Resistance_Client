using UnityEngine;

public class MonitorButtonController : MonoBehaviour
{
    [Header("Hierarchy")]
    [SerializeField] private GameObject _perkPanel;

    public void OnClickPerkButton()
    {
        LobbySceneManager.Instance.OpenUI(_perkPanel);
    }
}