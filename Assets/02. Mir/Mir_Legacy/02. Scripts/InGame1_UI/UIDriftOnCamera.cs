using UnityEngine;

public class UIDriftOnCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _driftMultiplier = 0.5f; // 드리프트 세기
    [SerializeField] private float _followSpeed = 3f;       // 반응 속도 (낮을수록 지연 느낌 강해짐)

    private Vector3 initialLocalPosition;
    private Vector3 targetOffset;
    private Vector3 lastCamPosition;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        lastCamPosition = _player.position;
    }

    void LateUpdate()
    {
        Vector3 camMovement = _player.position - lastCamPosition;

        // UI가 카메라 이동에 따라 반응하게 만드는 부분
        targetOffset = Vector3.Lerp(targetOffset, camMovement * _driftMultiplier, Time.deltaTime * _followSpeed);
        transform.localPosition = initialLocalPosition + targetOffset;

        lastCamPosition = _player.position;
    }
}
