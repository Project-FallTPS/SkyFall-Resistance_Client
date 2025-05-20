using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class Lobby_CinemachineCameraSwitcher : MonoBehaviour
{
    public static Lobby_CinemachineCameraSwitcher Instance;

    public CinemachineCamera MainCamera;
    public CinemachineCamera PlayerCam;
    public CinemachineCamera MonitorCam;

    void Awake()
    {
        Instance = this;
    }

    public void SwitchToCamera(LobbyCameraType type)
    {
        if (type == LobbyCameraType.MainSpot)
        {
            MainCamera.Priority = 10;
            PlayerCam.Priority = 1;
            MonitorCam.Priority = 1;
        }
        else if (type == LobbyCameraType.PlayerSpot)
        {
            MainCamera.Priority = 1;
            PlayerCam.Priority = 10;
            MonitorCam.Priority = 1;
        }
        else if (type == LobbyCameraType.MonitorSpot)
        {
            MainCamera.Priority = 1;
            PlayerCam.Priority = 1;
            MonitorCam.Priority = 10;
        }
    }
}
