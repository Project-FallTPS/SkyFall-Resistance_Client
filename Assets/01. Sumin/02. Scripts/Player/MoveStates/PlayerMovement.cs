using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    private const int MAX_JUMP_COUNT = 2;

    [Header("# Stat")]
    public PlayerStatHolder PlayerStatManager { get; private set; }

    [Header("# StateMachine")]
    public Dictionary<EPlayerMoveState, IPlayerState> StateDictionary { get; private set; }
    public IPlayerState CurrentState;

    [Header(" Movement Settings")]
    public float RotateSpeed = 10f;
    public float CurrentSpeed { get; private set; }
    public Vector3 MoveDirection { get; private set; }
    public bool IsSprint { get; private set; }

    [Header("# Jump")]
    private int _jumpCount = 0;   // 현재 점프 횟수

    [Header("# Components")]
    public Rigidbody Rigid { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    public Animator Animator { get; private set; }

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        PlayerStatManager = GetComponent<PlayerStatHolder>();
        Animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        MainCameraTransform = Camera.main.transform;
        StateDictionary = new Dictionary<EPlayerMoveState, IPlayerState>
        {
            { EPlayerMoveState.Ground, new PlayerGroundState() },
            { EPlayerMoveState.Airborne, new PlayerAirborneState() },
        };
        ChangeState(EPlayerMoveState.Airborne);
    }

    private void Start()
    {
        CurrentSpeed = PlayerStatManager.GetStat(EStatType.MoveSpeed);
    }

    private void Update()
    {
        //CheckStateTransition();
        CurrentState?.Update();
        if(IsSprint)
        {
            PlayerStatManager.RegenStamina();
        }
    }

    private void LateUpdate()
    {
        HandleRotation();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Animator.SetBool("anim_Player_IsGrounded", true);
            _jumpCount = 0; // 점프 횟수 초기화

            if (CurrentState is not PlayerGroundState)
            {
                ChangeState(EPlayerMoveState.Ground);
                PlayerEffectPoolManager.Instance.GetObject(EPlayerEffectType.LandingEffect, transform.position);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Animator.SetBool("anim_Player_IsGrounded", false);
        }
    }

    private void CheckStateTransition()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, LayerMask.NameToLayer("Ground"));
        
        if (isGrounded && CurrentState is PlayerAirborneState)
        {
            ChangeState(EPlayerMoveState.Ground);
            Animator.SetBool("anim_Player_IsGrounded", true);
        }
        //else if (!isGrounded && CurrentState is PlayerGroundState)
        //{
        //    ChangeState(EPlayerMoveState.Airborne);
        //    Animator.SetBool("anim_Player_IsGrounded", false);
        //}
    }

    public void HandleMovement(float h, float v, bool isKeyDown)
    {
        Animator.SetFloat("anim_Player_MovingX", h);
        Animator.SetFloat("anim_Player_MovingZ", v);
        Animator.SetBool("anim_Player_IsMoving", isKeyDown || (!Mathf.Approximately(v, 0f) || !Mathf.Approximately(h, 0f)));
        CurrentState?.HandleMovement(h, v, isKeyDown);
    }

    public void SetSprint(bool isSprint)
    {
        CurrentSpeed = isSprint ? PlayerStatManager.GetStat(EStatType.SprintSpeed) : PlayerStatManager.GetStat(EStatType.MoveSpeed);
        IsSprint = isSprint;
        Animator.SetBool("anim_Player_IsBoosting", isSprint);
    }

    public void ChangeState(EPlayerMoveState newState)
    {
        if (StateDictionary.TryGetValue(newState, out IPlayerState state))
        {
            Debug.Log($"{CurrentState} => {state}");
            if (CurrentState != null && CurrentState != state)
            {
                CurrentState.Exit(this);
            }
            CurrentState = state;
            CurrentState.Enter(this);        
        }
    }

    public void Jump()
    {
        if (_jumpCount >= MAX_JUMP_COUNT || CurrentState is not PlayerGroundState)
        {
            return;
        }

        Rigid.AddForce(Vector3.up * PlayerStatManager.GetStat(EStatType.JumpPower), ForceMode.Impulse);
        _jumpCount++;

        if(_jumpCount == 1)
        {
            Animator.SetTrigger("anim_Player_GroundJump");
        }
        else
        {
            Animator.SetTrigger("anim_Player_GroundDoubleJump");
            ChangeState(EPlayerMoveState.Airborne);
        }
    }


    public void Dodge(float h, float v)
    {

    }

    private void HandleRotation()
    {
        Vector3 camForward = MainCameraTransform.forward;

        if (CurrentState is PlayerGroundState)
        {
            // y축 제거해서 수평 방향만 사용
            camForward.y = 0f;
        }

        camForward.Normalize();

        if (camForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, RotateSpeed * Time.deltaTime);
        }
    }
}