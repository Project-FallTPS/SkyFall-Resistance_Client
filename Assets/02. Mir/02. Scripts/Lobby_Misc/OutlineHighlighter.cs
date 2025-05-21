using UnityEngine;

[RequireComponent(typeof(Outline))]
public class OutlineHighlighter : MonoBehaviour
{
    [Header("��ȯ�� ī�޶� ����")]
    public LobbyCameraType cameraType = LobbyCameraType.MainSpot;

    private Outline outline;
    private bool isHighlighted = false;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false; // �⺻�� ����
    }

    void OnMouseEnter()
    {
        outline.enabled = true;
        isHighlighted = true;
    }

    void OnMouseExit()
    {
        outline.enabled = false;
        isHighlighted = false;
    }
    
    void OnMouseDown()
    {
        if (isHighlighted && cameraType != LobbyCameraType.MainSpot)
        {
            Lobby_CinemachineCameraSwitcher.Instance.SwitchToCamera(cameraType);
        }
    }
}