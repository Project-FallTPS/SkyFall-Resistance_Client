using UnityEngine;

public class PlayerGroundState : IPlayerState
{
    private PlayerMovement _player = null;
    private Vector3 _moveDirection = new Vector3();

    public void Enter(PlayerMovement player)
    {
        if (_player == null)
        {
            _player = player;
        }
        _player.Rigid.linearVelocity = Vector3.zero;
        _player.Rigid.useGravity = true;
        _player.Rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        SetSprint(false);
    }

    public void Exit(PlayerMovement player)
    {
        _player.Rigid.linearVelocity = Vector3.zero;
        _player.Rigid.useGravity = false;
        _player.Rigid.constraints = RigidbodyConstraints.None;
    }

    public void HandleMovement(float h, float v, bool isKeyDown)
    {
        float threshold = 0.1f;

        float rawH = Mathf.Abs(h) < threshold ? 0 : Mathf.Sign(h);
        float rawV = Mathf.Abs(v) < threshold ? 0 : Mathf.Sign(v);

        Vector3 camForward = _player.MainCameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = _player.MainCameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        _moveDirection = (camForward * rawV + camRight * rawH).normalized;

        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            if (!_player.IsSprint || !_player.PlayerStatManager.TryUseStamina(EStatType.SprintStaminaUseRate))
            {
                _player.SetSprint(false);
            }

            Vector3 horizontalMove = _moveDirection * _player.CurrentSpeed * Time.fixedDeltaTime;
            Vector3 newPosition = _player.Rigid.position + horizontalMove;

            _player.Rigid.MovePosition(newPosition);
        }
    }

    public void SetSprint(bool isSprint)
    {
        _player.CurrentSpeed = isSprint ? _player.PlayerStatManager.GetStat(EStatType.GroundSprintSpeed) : _player.PlayerStatManager.GetStat(EStatType.GroundMoveSpeed);
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
        camForward.y = 0f;
        camForward.Normalize();

        if (camForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(camForward);
            _player.transform.rotation = Quaternion.Slerp(_player.transform.rotation, targetRot, _player.RotateSpeed * Time.deltaTime);
        }
    }
}