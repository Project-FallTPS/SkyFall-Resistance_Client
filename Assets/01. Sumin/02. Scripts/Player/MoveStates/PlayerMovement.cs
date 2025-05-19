using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 10f;

    private CharacterController _controller;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        Transform cam = Camera.main.transform;

        // 카메라 기준으로 평면 방향 벡터 계산
        Vector3 camForward = cam.forward;
        //camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        //camRight.y = 0;
        camRight.Normalize();

        // 실제 이동 방향 계산
        _moveDirection = (camForward * v + camRight * h).normalized;

        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            // 이동
            _controller.Move(_moveDirection * moveSpeed * Time.deltaTime);

            // 회전
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
