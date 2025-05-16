using UnityEngine;

public interface IPlayerState
{
    public void Enter(PlayerMovement player);
    public void Execute(PlayerMovement player);
    public void Exit(PlayerMovement player);
}