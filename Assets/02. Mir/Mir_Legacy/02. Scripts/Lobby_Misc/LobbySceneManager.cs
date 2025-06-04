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

    public Stack<UI_Popup> OpenUIStack = new Stack<UI_Popup>();

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
            if(OpenUIStack.Count <= 0)
            {
                SwitchToCamera(ELobbyCameraType.MainSpot);
            }
            else
            {
                CloseUI();
            }
        }
    }

    public void SwitchToCamera(ELobbyCameraType type)
    {
        if (type == ELobbyCameraType.MainSpot)
        {
            MainCamera.Priority = 10;
            PlayerCam.Priority = 1;
            MonitorCam.Priority = 1;

            _monitorUI.SetActive(false);
            _characterUI.SetActive(false);
        }
        else if (type == ELobbyCameraType.PlayerSpot)
        {
            MainCamera.Priority = 1;
            PlayerCam.Priority = 10;
            MonitorCam.Priority = 1;

            _monitorUI.SetActive(false);
            _characterUI.SetActive(true);
        }
        else if (type == ELobbyCameraType.MonitorSpot)
        {
            MainCamera.Priority = 1;
            PlayerCam.Priority = 1;
            MonitorCam.Priority = 10;

            _monitorUI.SetActive(true);
            _characterUI.SetActive(false);
        }
    }

    public void OpenUI(UI_Popup ui)
    {
        ui.Open();
        OpenUIStack.Push(ui);
    }

    public void CloseUI()
    {
        UI_Popup ui = OpenUIStack.Pop();
        ui.Close();
    }

    public void OnClickedGameStart()
    {
        Debug.Log("Scene Load");
        SceneTransitionManager.Instance.LoadScene(nameof(ESceneNames.FallScene));
    }
}
