using UnityEngine;

public interface IPlayerState
{
    public void Enter(PlayerMovement player);
    public void Execute(PlayerMovement player);
    public void Exit(PlayerMovement player);
    public void Update();
    public void HandleMovement();
}