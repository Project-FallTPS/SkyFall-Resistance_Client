using UnityEngine;

public interface IPlayerState
{
    public void Enter(PlayerMovement player);
    public void Exit(PlayerMovement player);
    public void Update();
    public void HandleMovement(float h, float v, bool isKeyDown);
    public void SetSprint(bool flag);
}