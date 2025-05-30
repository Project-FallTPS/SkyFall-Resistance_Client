using Unity.Cinemachine;
using UnityEngine;

public class CustomAxisFeeder : MonoBehaviour
{
    [Header("Cinemachine Component")]
    public CinemachinePanTilt PanTilt;

    [Header("Rotation Sensitivity")]
    public float PanSensitivity = 50f;
    public float TiltSensitivity = 30f;

    [Header("Smoothing Settings")]
    public float SmoothTime = 0.1f;

    [Header("Offset (중앙 위치 설정)")]
    public float PanOffset = -14f;
    public float TiltOffset = 0f;

    [Header("Angle Clamp")]
    public float PanMin = -25f;
    public float PanMax = 25f;
    public float TiltMin = -5f;
    public float TiltMax = 5f;

    private float _panValue, _tiltValue;
    private float _panVelocity, _tiltVelocity;

    void Update()
    {
        float mouseX = (Input.mousePosition.x / Screen.width - 0.5f) * 2f;
        float mouseY = (Input.mousePosition.y / Screen.height - 0.5f) * 2f;

        float targetPan = mouseX * PanSensitivity;
        float targetTilt = -mouseY * TiltSensitivity;

        _panValue = Mathf.SmoothDamp(_panValue, targetPan, ref _panVelocity, SmoothTime);
        _tiltValue = Mathf.SmoothDamp(_tiltValue, targetTilt, ref _tiltVelocity, SmoothTime);

        float clampedPan = Mathf.Clamp(_panValue + PanOffset, PanMin, PanMax);
        float clampedTilt = Mathf.Clamp(_tiltValue + TiltOffset, TiltMin, TiltMax);

        PanTilt.PanAxis.Value = clampedPan;
        PanTilt.TiltAxis.Value = clampedTilt;
    }
}
