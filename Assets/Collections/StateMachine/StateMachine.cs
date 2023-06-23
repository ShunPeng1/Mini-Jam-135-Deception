using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Collections.StateMachine
{
    public abstract class StateMachine<TStateEnum> : MonoBehaviour where TStateEnum : Enum
    {
        [Header("State Machine ")]
        [SerializeField] public TStateEnum MyStateEnum;

        protected List<Func<TStateEnum,object[],IEnumerator>> OnEnterEvents = new();
        protected List<Func<TStateEnum,object[],IEnumerator>> OnExitEvents = new();
    
        
        public enum StateEvent
        {
            OnEnter,
            OnExit
        }

        public IEnumerator OnExitState(TStateEnum enterState = default, object [] parameters = null)
        {
            foreach (var exitEnumerator in OnExitEvents)
            {
                yield return StartCoroutine(exitEnumerator.Invoke(enterState, parameters));
            }
        }
        
        public IEnumerator OnEnterState(TStateEnum exitState = default, object [] parameters = null)
        {
            foreach (var enterEnumerator in OnEnterEvents)
            {
                yield return StartCoroutine(enterEnumerator.Invoke(exitState, parameters));
            }
        }

    }
    
    public class StateMachine<TMonoBehavior, TStateEnum> : StateMachine<TStateEnum>
        where TMonoBehavior : StateMachine<TStateEnum>
        where TStateEnum : Enum 
    {
        public void AddToFunctionQueue(Action<TStateEnum> action, StateEvent stateEvent)
        {
            switch (stateEvent)
            {
                case StateEvent.OnEnter:
                    OnEnterEvents.Add((stateEnum,_) => ConvertToIEnumerator(action, stateEnum));
                    break;
                case StateEvent.OnExit:
                    OnExitEvents.Add((stateEnum,_) => ConvertToIEnumerator(action, stateEnum));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        
        }
        
        public void AddToFunctionQueue(Action<TStateEnum, object[]> action, StateEvent stateEvent)
        {
            switch (stateEvent)
            {
                case StateEvent.OnEnter:
                    OnEnterEvents.Add((stateEnum, parameters) => ConvertToIEnumerator(action, stateEnum, parameters));
                    break;
                case StateEvent.OnExit:
                    OnExitEvents.Add((stateEnum, parameters) => ConvertToIEnumerator(action, stateEnum, parameters));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        
        }
        
        public void AddToFunctionQueue(Func<TStateEnum,object[], IEnumerator> coroutine, StateEvent stateEvent)
        {
            switch (stateEvent)
            {
                case StateEvent.OnEnter:
                    OnEnterEvents.Add(coroutine);
                    break;
                case StateEvent.OnExit:
                    OnExitEvents.Add(coroutine);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        }

        public void AddToFunctionQueue(Tween tween, StateEvent stateEvent)
        {
            switch (stateEvent)
            {
                case StateEvent.OnEnter:
                    OnEnterEvents.Add((_,_) => ConvertToIEnumerator(tween));
                    break;
                case StateEvent.OnExit:
                    OnExitEvents.Add((_,_) => ConvertToIEnumerator(tween));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stateEvent), stateEvent, null);
            }
        }
        
        private IEnumerator ConvertToIEnumerator(Action<TStateEnum> action, TStateEnum stateEnum)
        {
            action.Invoke(stateEnum);
            yield return null;
        }
        
        private IEnumerator ConvertToIEnumerator(Action<TStateEnum, object[]> action, TStateEnum stateEnum, object[] parameters = null)
        {
            action.Invoke(stateEnum, parameters);
            yield return null;
        }
        
        private IEnumerator ConvertToIEnumerator(Tween tween)
        {   
            yield return tween.WaitForCompletion(); //Intentionally make them to Coroutine
        }

    }
}