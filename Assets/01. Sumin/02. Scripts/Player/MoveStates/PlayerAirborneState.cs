using UnityEngine;

public class PlayerAirborneState : IPlayerState
{
    public void Enter(PlayerMovement player)
    {
        //player.CurrentSpeed = PlayerStatManager.Instance.GetStat(EStatType.MoveSpeed);
    }

    public void Execute(PlayerMovement player)
    {
        if (player.MoveDirection.sqrMagnitude > 0.01f)
        {
            player.CharacterController.Move(player.MoveDirection * player.CurrentSpeed * Time.deltaTime);
        }
    }

    public void Exit(PlayerMovement player)
    {
        //player.CurrentSpeed = 0;
    }

    public void HandleMovement()
    {
        throw new System.NotImplementedException();
    }

    public void Update()
    {
        throw new System.NotImplementedException();
    }
}