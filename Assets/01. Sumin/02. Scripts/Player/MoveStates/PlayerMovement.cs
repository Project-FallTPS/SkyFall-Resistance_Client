using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;                 // ★ DOTween

[System.Serializable]
public class Jetpack             // struct → class (참조형)
{
    public ParticleSystem BigSmoke;
    [HideInInspector] public Tween RateTween; // 현재 진행 중인 트윈
}

public class PlayerMovement : MonoBehaviour
{
    private readonly int MAX_JUMP_COUNT = 2;

    // ──────────────────────────────────────────────────────
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
    private int _jumpCount = 0;

    [Header("# Components")]
    public Rigidbody Rigid { get; private set; }
    public Transform MainCameraTransform { get; private set; }
    public Animator Animator { get; private set; }

    [Header("# Jetpack")]
    [SerializeField] private List<Jetpack> Jetpacks;
    public bool HasOverloadJetpack { get; private set; }

    [Header("# Game Feel Action Line")]
    [SerializeField] private MMF_Player _sprintActionLine;
    // 트윈용 상수
    private const float TARGET_RATE = 60f;
    private const float LERP_DURATION = 0.7f;

    // Feel 용 Flag
    private bool _hasPlayedSprintActionLine;   // 이미 선언돼 있음
    private bool _sprintActionLineReady;       // 새 플래그

    // ──────────────────────────────────────────────────────
    private void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        PlayerStatManager = GetComponent<PlayerStatHolder>();
        Animator = GetComponentInChildren<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        MainCameraTransform = Camera.main.transform;

        StateDictionary = new()
        {
            { EPlayerMoveState.Ground,    new PlayerGroundState() },
            { EPlayerMoveState.Airborne,  new PlayerAirborneState() },
        };

        ChangeState(EPlayerMoveState.Airborne);
        _sprintActionLine?.Initialization();    // 강제 초기화
        _sprintActionLineReady = true;          // 준비 완료
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
        if (!IsSprint || HasOverloadJetpack) PlayerStatManager.RegenStamina();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Animator.SetBool("anim_Player_IsGrounded", true);
            _jumpCount = 0;

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
            Animator.SetBool("anim_Player_IsGrounded", false);
    }

    public void HandleMovement(float h, float v, bool isKeyDown)
    {
        Animator.SetFloat("anim_Player_MovingX", h);
        Animator.SetFloat("anim_Player_MovingZ", v);
        Animator.SetBool("anim_Player_IsMoving",
            isKeyDown || (!Mathf.Approximately(v, 0f) || !Mathf.Approximately(h, 0f)));

        CurrentState?.HandleMovement(h, v, isKeyDown);
    }

    // ──────────────────────────────────────────────────────
    /// <summary>스프린트 토글 + 제트팩 연기 트윈</summary>
    public void SetSprint(bool isSprint)
    {
        if (HasOverloadJetpack) isSprint = true;

        CurrentState?.SetSprint(isSprint);
        IsSprint = isSprint;

        // ───────────── Feel Action Line ─────────────
        if (_sprintActionLineReady)
        {
            if (isSprint)
            {
                if (!_hasPlayedSprintActionLine)
                {
                    _sprintActionLine.PlayFeedbacks();
                    _hasPlayedSprintActionLine = true;
                }
            }
            else
            {
                if (_hasPlayedSprintActionLine)   // 안전: 최소 1회 Play 이후에만 Stop
                {
                    _sprintActionLine.StopFeedbacks();
                    _hasPlayedSprintActionLine = false;
                }
            }
        }

        // ───────────── Jetpack Smoke Tween ─────────────
        float targetRate = isSprint ? TARGET_RATE : 0f;

        foreach (var jp in Jetpacks)
        {
            jp.RateTween?.Kill();

            var ps = jp.BigSmoke;
            var emission = ps.emission;
            float start = emission.rateOverTime.constant;

            if (!ps.isPlaying && targetRate > 0f) ps.Play();

            jp.RateTween = DOTween.To(
                    () => start,
                    x => emission.rateOverTime = x,
                    targetRate,
                    LERP_DURATION)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    if (Mathf.Approximately(targetRate, 0f))
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    jp.RateTween = null;
                });
        }
    }

    // ──────────────────────────────────────────────────────
    public void ChangeState(EPlayerMoveState newState)
    {
        if (StateDictionary.TryGetValue(newState, out IPlayerState state))
        {
            Debug.Log($"{CurrentState} => {state}");
            if (CurrentState != null && CurrentState != state) CurrentState.Exit(this);
            CurrentState = state;
            CurrentState.Enter(this);
        }
    }

    public void Jump()
    {
        if (_jumpCount >= MAX_JUMP_COUNT || CurrentState is not PlayerGroundState) return;

        Rigid.AddForce(Vector3.up * PlayerStatManager.GetStat(EStatType.JumpPower), ForceMode.Impulse);
        _jumpCount++;

        if (_jumpCount == 1)
            Animator.SetTrigger("anim_Player_GroundJump");
        else
        {
            Animator.SetTrigger("anim_Player_GroundDoubleJump");
            ChangeState(EPlayerMoveState.Airborne);
        }
    }
}
