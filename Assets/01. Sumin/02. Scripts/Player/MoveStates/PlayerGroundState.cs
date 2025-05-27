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
        Vector3 camForward = _player.MainCameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = _player.MainCameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        _moveDirection = (camForward * v + camRight * h).normalized;

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
        // 공중 상태에서의 지속적인 업데이트가 필요한 경우 여기에 구현
        // 예: 공중에서의 회전, 특수 동작 등
    }
}