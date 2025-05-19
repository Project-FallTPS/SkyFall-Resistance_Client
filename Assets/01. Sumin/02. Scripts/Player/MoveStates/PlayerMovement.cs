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

        // ī�޶� �������� ��� ���� ���� ���
        Vector3 camForward = cam.forward;
        //camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cam.right;
        //camRight.y = 0;
        camRight.Normalize();

        // ���� �̵� ���� ���
        _moveDirection = (camForward * v + camRight * h).normalized;

        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            // �̵�
            _controller.Move(_moveDirection * moveSpeed * Time.deltaTime);

            // ȸ��
            Quaternion targetRotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}
