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
        //_player.Rigid.linearVelocity = Vector3.zero;
        _player.Rigid.useGravity = true;
        _player.Rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Exit(PlayerMovement player)
    {
        //_player.Rigid.linearVelocity = Vector3.zero;
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

        Vector3 currentVelocity = _player.Rigid.linearVelocity;

        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            if (!_player.IsSprint || !_player.PlayerStatManager.TryUseStamina(EStatType.SprintStaminaUseRate))
            {
                _player.SetSprint(false);
            }

            Vector3 moveVelocity = _moveDirection * _player.CurrentSpeed;
            moveVelocity.y = currentVelocity.y; // 중력 영향 유지
            _player.Rigid.linearVelocity = moveVelocity;
        }
        else
        {
            // 이동 입력이 없을 때도 중력은 유지
            _player.Rigid.linearVelocity = new Vector3(0, currentVelocity.y, 0);
        }
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