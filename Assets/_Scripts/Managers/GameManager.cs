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
        InitState,
        LoseState
    }

    public class GameManager : StateMachineManager<GameManager,GameStateEnum>
    {
        [SerializeField] private List<StateMachine<GameStateEnum>> _stateMachines;
        [SerializeField] private StateMachine<GameStateEnum> _startStateMachine;
        private void Start()
        {
            foreach (var stateMachine in _stateMachines)
            {
                AddState(stateMachine.MyStateEnum, stateMachine);
            }

            currentStateMachine = _startStateMachine;
            StartCoroutine(currentStateMachine.OnEnterState());
        }

    }
}