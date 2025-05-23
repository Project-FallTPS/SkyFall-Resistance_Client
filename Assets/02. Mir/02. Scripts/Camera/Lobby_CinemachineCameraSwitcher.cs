using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class Lobby_CinemachineCameraSwitcher : MonoBehaviour
{
    public static Lobby_CinemachineCameraSwitcher Instance;

    public CinemachineCamera MainCamera;
    public CinemachineCamera PlayerCam;
    public CinemachineCamera MonitorCam;

    [SerializeField] private GameObject _monitorUI;
    [SerializeField] private GameObject _characterUI;

    protected private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        MainCamera.Priority = 10;
        PlayerCam.Priority = 1;
        MonitorCam.Priority = 1;
    }

    private void Start()
    {
        if (_monitorUI == null || _characterUI == null) Debug.LogError("UI is Not Assigned");
        else
        {
            _monitorUI.SetActive(false);
            _characterUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SwitchToCamera(LobbyCameraType.MainSpot);
    }

    public void SwitchToCamera(LobbyCameraType type)
    {
        if (type == LobbyCameraType.MainSpot)
        {
            MainCamera.Priority = 10;
            PlayerCam.Priority = 1;
            MonitorCam.Priority = 1;

            _monitorUI.SetActive(false);
            _characterUI.SetActive(false);
        }
        else if (type == LobbyCameraType.PlayerSpot)
        {
            MainCamera.Priority = 1;
            PlayerCam.Priority = 10;
            MonitorCam.Priority = 1;

            _monitorUI.SetActive(false);
            _characterUI.SetActive(true);
        }
        else if (type == LobbyCameraType.MonitorSpot)
        {
            MainCamera.Priority = 1;
            PlayerCam.Priority = 1;
            MonitorCam.Priority = 10;

            _monitorUI.SetActive(true);
            _characterUI.SetActive(false);
        }
    }
}
