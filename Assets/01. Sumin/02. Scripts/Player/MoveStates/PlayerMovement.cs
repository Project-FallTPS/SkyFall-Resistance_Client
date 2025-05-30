using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct Jetpack
{
    public ParticleSystem BigSmoke;
}

public class PlayerMovement : MonoBehaviour
{
    private readonly int MAX_JUMP_COUNT = 2;

    [Header("# Stat")]
    public PlayerStatHolder PlayerStatManager { get; private set; }

    [Header("# StateMachine")]
    public Dictionary<EPlayerMoveState, IPlayerState> StateDictionary { get; private set; }
    public IPlayerState CurrentState;

    [Header(" Movement Settings")]
    public float RotateSpeed = 10f;
    public float CurrentSpeed { get; set; }
    public Vector3 MoveDirection { get; set; }
    public bool IsSprint { get; set; }

    [Header("# Jump")]
    private int _jumpCount = 0;   // 현재 점프 횟수

    [Header("# Components")]
    public Rigidbody Rigid { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    public Animator Animator { get; private set; }

    [Header("# Jetpack")]
    [SerializeField] private List<Jetpack> Jetpacks;
    public bool HasOverloadJetpack { get; private set; }

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
        SetSprint(false);
    }

    private void Start()
    {
        CurrentSpeed = PlayerStatManager.GetStat(EStatType.MoveSpeed);
        HasOverloadJetpack = PerkManager.Instance.EquippedPerks.ContainsKey(EPerkType.OverloadJetPack);
    }

    private void Update()
    {
        CurrentState?.Update();
        if (!IsSprint || HasOverloadJetpack)
        {
            PlayerStatManager.RegenStamina();
        }
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

    public void HandleMovement(float h, float v, bool isKeyDown)
    {
        Animator.SetFloat("anim_Player_MovingX", h);
        Animator.SetFloat("anim_Player_MovingZ", v);
        Animator.SetBool("anim_Player_IsMoving", isKeyDown || (!Mathf.Approximately(v, 0f) || !Mathf.Approximately(h, 0f)));
        CurrentState?.HandleMovement(h, v, isKeyDown);
    }

    public void SetSprint(bool isSprint)
    {
        if (HasOverloadJetpack)
        {
            isSprint = true;
        }

        CurrentState?.SetSprint(isSprint);

        foreach (var smoke in Jetpacks)
        {
            if (isSprint)
            {
                smoke.BigSmoke.Play();
            }
            else
            {
                smoke.BigSmoke.Stop();
            }
        }
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
}