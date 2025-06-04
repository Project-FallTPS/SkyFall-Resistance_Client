using UnityEngine;

[RequireComponent(typeof(Outline))]
public class OutlineHighlighter : MonoBehaviour
{
    [Header("전환할 카메라 선택")]
    public ELobbyCameraType cameraType;

    private Outline _outline;
    private bool isHighlighted = false;

    void Start()
    {
        _outline = GetComponent<Outline>();
        _outline.enabled = false; // 기본은 꺼둠
    }

    void OnMouseEnter()
    {
        _outline.enabled = true;
        isHighlighted = true;
    }

    void OnMouseExit()
    {
        _outline.enabled = false;
        isHighlighted = false;
    }
    
    void OnMouseDown()
    {
        if (isHighlighted && cameraType != ELobbyCameraType.MainSpot) LobbySceneManager.Instance.SwitchToCamera(cameraType);
    }

    public void EnableOutline() => _outline.enabled = true;
    public void DisableOutline() => _outline.enabled = false;
    public void TriggerAction()
    {
        if (cameraType != ELobbyCameraType.MainSpot)
            LobbySceneManager.Instance.SwitchToCamera(cameraType);
    }
}