using System.Collections.Generic;
using Collections.StateMachine;
using UnityEngine;

namespace _Scripts.Managers
{
    public enum GameStateEnum
    {
        NormalState,
        PauseState,
        GhostState,
        InitNormalState,
        InitGhostState,
        LoseState
    }

    public class GameManager : StateMachineManager<GameManager,GameStateEnum>
    {
        [SerializeField] private List<StateMachine<GameStateEnum>> _stateMachines;
        private void Awake()
        {
            foreach (var stateMachine in _stateMachines)
            {
                AddState(stateMachine.MyStateEnum, stateMachine);
            }
            
            StartCoroutine(CurrentStateMachine.OnEnterState());
        }

    }
}