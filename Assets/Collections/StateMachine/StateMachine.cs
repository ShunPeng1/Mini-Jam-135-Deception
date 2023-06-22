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
        [SerializeField] public TStateEnum _myStateEnum;

        protected List<Func<TStateEnum,object[],IEnumerator>> onEnterEvents = new();
        protected List<Func<TStateEnum,object[],IEnumerator>> onExitEvents = new();
    
        
        public enum StateEvent
        {
            OnEnter,
            OnExit
        }

        public IEnumerator OnExitState(TStateEnum enterState = default, object [] parameters = null)
        {
            foreach (var exitEnumerator in onExitEvents)
            {
                yield return StartCoroutine(exitEnumerator.Invoke(enterState, parameters));
            }
        }
        
        public IEnumerator OnEnterState(TStateEnum exitState = default, object [] parameters = null)
        {
            foreach (var enterEnumerator in onEnterEvents)
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
                    onEnterEvents.Add((stateEnum,_) => ConvertToIEnumerator(action, stateEnum));
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add((stateEnum,_) => ConvertToIEnumerator(action, stateEnum));
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
                    onEnterEvents.Add((stateEnum, parameters) => ConvertToIEnumerator(action, stateEnum, parameters));
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add((stateEnum, parameters) => ConvertToIEnumerator(action, stateEnum, parameters));
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
                    onEnterEvents.Add(coroutine);
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add(coroutine);
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
                    onEnterEvents.Add((_,_) => ConvertToIEnumerator(tween));
                    break;
                case StateEvent.OnExit:
                    onExitEvents.Add((_,_) => ConvertToIEnumerator(tween));
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