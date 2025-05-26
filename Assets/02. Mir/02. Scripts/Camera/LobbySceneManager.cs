using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class LobbySceneManager : Singleton<LobbySceneManager>
{
    public CinemachineCamera MainCamera;
    public CinemachineCamera PlayerCam;
    public CinemachineCamera MonitorCam;

    [SerializeField] private GameObject _monitorUI;
    [SerializeField] private GameObject _characterUI;

    public Stack<GameObject> OpenUIStack = new Stack<GameObject>();

    protected override void Awake()
    {
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(OpenUIStack.Count == 0)
            {
                SwitchToCamera(LobbyCameraType.MainSpot);
            }
            else
            {
                GameObject ui = OpenUIStack.Pop();
                ui.SetActive(false);
            }
        }
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

    public void OpenUI(GameObject ui)
    {
        ui.SetActive(true);
        OpenUIStack.Push(ui);
    }
}
