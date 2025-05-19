using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMoveFunction : MonoBehaviour
{
    [Header(" Movement Settings")]
    public float rotateSpeed = 10f;
    public float CurrentSpeed;
    public Vector3 MoveDirection { get; private set; }

    [Header("# Components")]
    public CharacterController _characterController { get; private set; }
    private Transform _mainCameraTransform;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _mainCameraTransform = Camera.main.transform;
    }

    private void Start()
    {
        CurrentSpeed = PlayerStatManager.Instance.GetStat(EStatType.MoveSpeed);
    }

    private void LateUpdate()
    {
        HandleRotation();
    }

    public void HandleDirection(float h, float v)
    {
        Vector3 camForward = _mainCameraTransform.forward;
        camForward.Normalize();

        Vector3 camRight = _mainCameraTransform.right;
        camRight.Normalize();

        MoveDirection = (camForward * v + camRight * h).normalized;
    }

    public void HandleMovement(float h, float v)
    {
        Vector3 camForward = _mainCameraTransform.forward;
        camForward.Normalize();

        Vector3 camRight = _mainCameraTransform.right;
        camRight.Normalize();

        MoveDirection = (camForward * v + camRight * h).normalized;

        if (MoveDirection.sqrMagnitude > 0.01f)
        {
            _characterController.Move(MoveDirection * CurrentSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        Vector3 camForward = _mainCameraTransform.forward;

        if (camForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
    }
}
