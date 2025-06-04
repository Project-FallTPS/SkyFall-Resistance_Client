using UnityEngine;

public class PlayerAirborneState : IPlayerState
{
    private PlayerMovement _player = null;
    private Vector3 _moveDirection = new Vector3();

    public void Enter(PlayerMovement player)
    {
        if (_player == null)
        {
            _player = player;
        }
        //_player.Rigid.linearVelocity = Vector3.zero;
        _player.Rigid.useGravity = false;
        _player.Rigid.constraints = RigidbodyConstraints.None;
        _player.Rigid.linearVelocity = Vector3.zero;
        SetSprint(false);
    }

    public void Exit(PlayerMovement player)
    {
        _player.Rigid.linearVelocity = Vector3.zero;
    }

    public void HandleMovement(float h, float v, bool isKeyDown)
    {
        Vector3 camForward = _player.MainCameraTransform.forward;
        camForward.Normalize();

        Vector3 camRight = _player.MainCameraTransform.right;
        camRight.Normalize();

        _moveDirection = (camForward * v + camRight * h).normalized;
        _player.MoveDirection = _moveDirection;
        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            if (!_player.IsSprint)
            {
                _player.SetSprint(false); // 보통 이동
            }
            else
            {
                if (!_player.HasOverloadJetpack && !_player.PlayerStatManager.TryUseStamina(EStatType.SprintStaminaUseRate))
                {
                    _player.SetSprint(false);
                }
            }
            Vector3 moveOffset = _moveDirection * _player.CurrentSpeed * Time.fixedDeltaTime;
            _player.Rigid.MovePosition(_player.Rigid.position + moveOffset);
        }
        else
        {
            _player.Rigid.linearVelocity = Vector3.zero;
        }
    }

    public void SetSprint(bool isSprint)
    {
        _player.CurrentSpeed = isSprint ? _player.PlayerStatManager.GetStat(EStatType.SprintSpeed) : _player.PlayerStatManager.GetStat(EStatType.MoveSpeed);
        _player.IsSprint = isSprint;
        _player.Animator.SetBool("anim_Player_IsBoosting", isSprint);
    }

    public void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        Vector3 camForward = _player.MainCameraTransform.forward;
        camForward.Normalize();

        if (camForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            _player.transform.rotation = Quaternion.RotateTowards(
                _player.transform.rotation,
                targetRot,
                _player.RotateSpeed * Time.deltaTime
            );
        }
    }
}