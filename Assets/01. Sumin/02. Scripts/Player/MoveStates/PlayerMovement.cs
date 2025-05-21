using UnityEngine;

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
    private Animator _animator;
    private Rigidbody _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _playerStatManager = GetComponent<PlayerStatHolder>();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
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

    //public void HandleMovement(float h, float v)
    //{
    //    _animator.SetBool("anim_Player_IsMoving", (h != 0 || v != 0));
    //    _animator.SetFloat("anim_Player_MovingZ", v);
    //    _animator.SetFloat("anim_Player_MovingX", h);
    //    //h = Mathf.Sign(h);
    //    //v = Mathf.Sign(v);
    //    Vector3 camForward = _mainCameraTransform.forward;
    //    camForward.Normalize();

    //    Vector3 camRight = _mainCameraTransform.right;
    //    camRight.Normalize();

    //    MoveDirection = (camForward * v + camRight * h).normalized;

    //    if (MoveDirection.sqrMagnitude > 0.01f)
    //    {
    //        if(!_isSprint /*|| _playerStatManager.TryUseStamina(EStatType.SprintStaminaUseRate)*/)
    //        {
    //            SetSprint(false);
    //        }
    //        _characterController.Move(MoveDirection * CurrentSpeed * Time.deltaTime);
    //        // TODO : 월드 Y각 특정 이상이면 다른 애니메이션 
    //    }
    //}

    public void HandleMovement(float h, float v)
    {
        _animator.SetFloat("anim_Player_MovingX", h);
        _animator.SetFloat("anim_Player_MovingZ", v);
        _animator.SetBool("anim_Player_IsMoving", Mathf.Sign(v) != 0 || Mathf.Sign(h) != 0);

        Vector3 camForward = _mainCameraTransform.forward;
        camForward.Normalize();

        Vector3 camRight = _mainCameraTransform.right;
        camRight.Normalize();

        MoveDirection = (camForward * v + camRight * h).normalized;

        if (MoveDirection.sqrMagnitude > 0.01f)
        {
            if (!_isSprint /*|| _playerStatManager.TryUseStamina(EStatType.SprintStaminaUseRate)*/)
            {
                SetSprint(false);
            }

            Vector3 targetPosition = transform.position + MoveDirection * CurrentSpeed * Time.deltaTime;
            _rigid.MovePosition(targetPosition);
        }
    }


    public void SetSprint(bool isSprint)
    {
        CurrentSpeed = isSprint ? _playerStatManager.GetStat(EStatType.SprintSpeed) : _playerStatManager.GetStat(EStatType.MoveSpeed);
        _isSprint = isSprint;
        _animator.SetBool("anim_Player_IsBoosting", isSprint);
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
