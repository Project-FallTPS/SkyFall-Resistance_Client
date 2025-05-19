using UnityEngine;

public class PlayerMoveState : IPlayerState
{
    public void Enter(PlayerMovement player)
    {
        //player.CurrentSpeed = PlayerStatManager.Instance.GetStat(EStatType.MoveSpeed);
    }

    public void Execute(PlayerMovement player)
    {
        if (player.MoveDirection.sqrMagnitude > 0.01f)
        {
            player._characterController.Move(player.MoveDirection * player.CurrentSpeed * Time.deltaTime);
        }
    }

    public void Exit(PlayerMovement player)
    {
        //player.CurrentSpeed = 0;
    }
}