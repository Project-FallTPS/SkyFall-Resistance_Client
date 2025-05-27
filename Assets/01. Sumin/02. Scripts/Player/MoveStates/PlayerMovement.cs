using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
    public CharacterController CharacterController { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    public Animator Animator { get; private set; }
    public Rigidbody Rigid { get; private set; }
    

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        _playerStatManager = GetComponent<PlayerStatHolder>();
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        MainCameraTransform = Camera.main.transform;
        StateMachine = new PlayerMoveStateMachine(this, new Dictionary<EPlayerMoveState, IPlayerState>
        {
            { EPlayerMoveState.Ground, new PlayerGroundState() },
            { EPlayerMoveState.Airborne, new PlayerAirborneState() },
        });
        ChangeState(EPlayerMoveState.Airborne);

        //if (SceneManager.GetActiveScene().name == "PlayerTest")
        //{
        //    _rigid.useGravity = true;
        //}
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
        Vector3 camForward = MainCameraTransform.forward;
        camForward.Normalize();

        Vector3 camRight = MainCameraTransform.right;
        camRight.Normalize();

        MoveDirection = (camForward * v + camRight * h).normalized;
    }

    public void HandleMovement(float h, float v, bool isKeyDown)
    {
        Animator.SetFloat("anim_Player_MovingX", h);
        Animator.SetFloat("anim_Player_MovingZ", v);
        Animator.SetBool("anim_Player_IsMoving", isKeyDown || (!Mathf.Approximately(v, 0f) || !Mathf.Approximately(h, 0f)));

        Vector3 camForward = MainCameraTransform.forward;
        //if (SceneManager.GetActiveScene().name == "PlayerTest") // 보스씬이라면 이라고 바꿔야댐
        //{
        //    camForward.y = 0;
        //}
        camForward.Normalize();

        Vector3 camRight = MainCameraTransform.right;
        //if (SceneManager.GetActiveScene().name == "PlayerTest") // 보스씬이라면 이라고 바꿔야댐
        //{
        //    camRight.y = 0;
        //}
        camRight.Normalize();

        MoveDirection = (camForward * v + camRight * h).normalized;

        if (MoveDirection.sqrMagnitude > 0.01f)
        {
            if (!_isSprint || _playerStatManager.TryUseStamina(EStatType.SprintStaminaUseRate))
            {
                SetSprint(false);
            }

            Vector3 targetPosition = transform.position + MoveDirection * CurrentSpeed * Time.deltaTime;
            Rigid.MovePosition(targetPosition);
        }
        else
        {
            Rigid.linearVelocity = Vector3.zero;
        }
    }

    public void SetSprint(bool isSprint)
    {
        CurrentSpeed = isSprint ? _playerStatManager.GetStat(EStatType.SprintSpeed) : _playerStatManager.GetStat(EStatType.MoveSpeed);
        _isSprint = isSprint;
        Animator.SetBool("anim_Player_IsBoosting", isSprint);
    }

    public void ChangeState(EPlayerMoveState state)
    {
        StateMachine.ChangeState(state);
    }

    private void HandleRotation()
    {
        Vector3 camForward = MainCameraTransform.forward;

        if (camForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, RotateSpeed * Time.deltaTime);
        }
    }
}
