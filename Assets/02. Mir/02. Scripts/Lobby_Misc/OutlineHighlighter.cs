using UnityEngine;

[RequireComponent(typeof(Outline))]
public class OutlineHighlighter : MonoBehaviour
{
    [Header("전환할 카메라 선택")]
    public LobbyCameraType cameraType;

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
        if (isHighlighted && cameraType != LobbyCameraType.MainSpot) Lobby_CinemachineCameraSwitcher.Instance.SwitchToCamera(cameraType);
    }
}