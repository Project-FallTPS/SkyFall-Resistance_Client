using UnityEngine;
using System.Collections;

public class UIDriftOnCamera : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _driftMultiplier = 0.5f; // 드리프트 세기
    [SerializeField] private float _followSpeed = 3f;       // 반응 속도 (낮을수록 지연 느낌 강해짐)

    private Vector3 initialLocalPosition;
    private Vector3 targetOffset;
    private Vector3 lastCamPosition;
    private bool isActive = true;

    void Start()
    {
        initialLocalPosition = transform.localPosition;
        lastCamPosition = _player.position;
    }

    void LateUpdate()
    {
        if (!isActive) return;

        Vector3 camMovement = _player.position - lastCamPosition;
        targetOffset = Vector3.Lerp(targetOffset, camMovement * _driftMultiplier, Time.deltaTime * _followSpeed);
        transform.localPosition = initialLocalPosition + targetOffset;

        lastCamPosition = _player.position;
    }

    // 외부에서 호출 가능한 비활성화 트리거
    public void TemporarilyDisable(float duration = 1f)
    {
        StartCoroutine(DisableTemporarily(duration));
    }

    private IEnumerator DisableTemporarily(float duration)
    {
        isActive = false;
        yield return new WaitForSeconds(duration);
        isActive = true;
    }
}