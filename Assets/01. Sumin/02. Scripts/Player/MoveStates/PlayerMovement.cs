using UnityEngine;
using System.Collections.Generic;
using static UnityEditor.Experimental.GraphView.GraphView;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("# Stat")]
    private PlayerStatHolder _playerStatManager;

    [Header("# StateMachine")]
    public PlayerMoveStateMachine StateMachine { get; private set; }

    [Header(" Movement Settings")]
    public float RotateSpeed = 10f;
    public float CurrentSpeed { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    private bool _isSprint;

    [Header("# Components")]
    public CharacterController _characterController { get; private set; }
    private Transform _mainCameraTransform;

    private void Awake()
    {
        _playerStatManager = GetComponent<PlayerStatHolder>();
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        _mainCameraTransform = Camera.main.transform;
        //StateMachine = new PlayerMoveStateMachine(this, new Dictionary<EPlayerMoveState, IPlayerState> 
        //{
        //    { EPlayerMoveState.Idle, new PlayerIdleState() },
        //    { EPlayerMoveState.Move, new PlayerMoveState() },
        //    { EPlayerMoveState.Sprint, new PlayerSprintState() },
        //    { EPlayerMoveState.TargetDash, new PlayerTargetDashState() },
        //});
    }

    private void Start()
    {
        CurrentSpeed = _playerStatManager.GetStat(EStatType.MoveSpeed);
    }

    private void Update()
    {
        //StateMachine.Update();
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
            if(!_isSprint || _playerStatManager.TryUseStamina(EStatType.SprintStaminaUseRate))
            {
                SetSprint(false);
            }
            _characterController.Move(MoveDirection * CurrentSpeed * Time.deltaTime);
        }
    }

    public void SetSprint(bool isSprint)
    {
        CurrentSpeed = isSprint ? _playerStatManager.GetStat(EStatType.SprintSpeed) : _playerStatManager.GetStat(EStatType.MoveSpeed);
        _isSprint = isSprint;
    }

    public void ChangeState(EPlayerMoveState state)
    {
        StateMachine.ChangeState(state);
    }

    private void HandleRotation()
    {
        Vector3 camForward = _mainCameraTransform.forward;

        if (camForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, RotateSpeed * Time.deltaTime);
        }
    }
}
