using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

namespace Collections.StateMachine
{
    public class StateMachineManager<TMonoBehaviour,TStateEnum> : SingletonMonoBehaviour<TMonoBehaviour> where TStateEnum : Enum where TMonoBehaviour : SingletonMonoBehaviour<TMonoBehaviour>
    {
        protected StateMachine<TStateEnum> currentStateMachine;
        private Dictionary<TStateEnum, StateMachine<TStateEnum>> _states = new ();

        private Queue<(StateMachine<TStateEnum>, object[], object[])> _changingStateQueue = new ();

        [Header("History")] 
        [SerializeField, Range(0,20)]private int _maxHistoryLength = 10;
        private LinkedList<(StateMachine<TStateEnum>, object[], object[])> _historyStates = new();
        
        private bool _isChangingState;
        
        public void AddState(TStateEnum stateEnum, StateMachine<TStateEnum> stateMachine)
        {
            _states[stateEnum] = stateMachine;
        }

        public void RemoveState(TStateEnum stateEnum)
        {
            _states.Remove(stateEnum);
        }

        public void SetState(TStateEnum stateEnum, object[] exitParameters = null, object[] enterParameters = null)
        {
            if (_states.TryGetValue(stateEnum, out StateMachine<TStateEnum> nextState))
            {
                //Debug.Log("State Machine Manager Enqueue state "+ nextState);
                _changingStateQueue.Enqueue((nextState, exitParameters, enterParameters));
                StartCoroutine(nameof(SwitchingState));
                AddStateToHistory(nextState, exitParameters, enterParameters);
            }
            else
            {
                Debug.LogWarning($"State {stateEnum} not found in state machine.");
            }
        }
        
        public void SetToLastState()
        {
            if (_historyStates.Count != 0)
            {
                _historyStates.RemoveFirst();
                _changingStateQueue.Enqueue(_historyStates.First.Value);
                StartCoroutine(nameof(SwitchingState));
            }
            else
            {
                Debug.LogError("No state in the history state machine");
            }
            
        }

        private void AddStateToHistory( StateMachine<TStateEnum> state, object[] exitParameters = null, object[] enterParameters = null)
        {
            if (_historyStates.Count >= _maxHistoryLength)
            {
                _historyStates.RemoveLast();
            }

            _historyStates.AddFirst((state, exitParameters, enterParameters));
        }
        
        public TStateEnum GetState()
        {
            return currentStateMachine._myStateEnum;
        }

        private IEnumerator SwitchingState()
        {
            if (_isChangingState)
            {
                yield break;
            }

            _isChangingState = true;
            while (_changingStateQueue.Count>0)
            {
                var info = _changingStateQueue.Dequeue();
                var nextState = info.Item1;
                object[] exitParameters = info.Item2, enterParameters = info.Item3 ;
                Debug.Log("State Machine Manager Change"+ currentStateMachine+ " To "+ nextState);

                yield return StartCoroutine(currentStateMachine.OnExitState(nextState._myStateEnum,exitParameters));
                yield return StartCoroutine(nextState.OnEnterState(currentStateMachine._myStateEnum,enterParameters));
                currentStateMachine = nextState;
            }

            _isChangingState = false;
        }
        
    }
}
