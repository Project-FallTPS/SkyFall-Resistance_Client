using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveStateMachine 
{
    private PlayerMovement _player;
    public Dictionary<EPlayerMoveState, IPlayerState> StateDictionary { get; private set; }
    public IPlayerState CurrentState { get; private set; }

    public PlayerMoveStateMachine(PlayerMovement player, Dictionary<EPlayerMoveState, IPlayerState> stateDictionary)
    {
        _player = player;
        StateDictionary = stateDictionary;
    }

    public void ChangeState(EPlayerMoveState newState)
    {
        if(StateDictionary.TryGetValue(newState, out IPlayerState state))
        {
            Debug.Log($"{CurrentState} => {state}");
            if(CurrentState != null && CurrentState != state)
            {
                CurrentState.Exit(_player);
            }
            CurrentState = state;
            CurrentState.Enter(_player);
        }
    }

    public IPlayerState GetState(EPlayerMoveState state)
    {
        return StateDictionary[state];
    }

    public void Update()
    {
        CurrentState?.Execute(_player);
    }

    public void ModifyState(EPlayerMoveState which, IPlayerState to)
    {
        StateDictionary[which] = to;
    }
}