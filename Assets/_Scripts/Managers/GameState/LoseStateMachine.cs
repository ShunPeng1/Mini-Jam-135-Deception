using System;
using System.Collections;
using Collections.StateMachine;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Managers.GameState
{
    public class LoseStateMachine : StateMachine<LoseStateMachine, GameStateEnum>
    {

        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _delayAppearDuration = 1f, _appearDuration = 1f;
        [SerializeField] private Ease _appearEase;

        private void Start()
        {
            AddToFunctionQueue(AppearUI, StateEvent.OnEnter);
        }

        private IEnumerator AppearUI(GameStateEnum stateEnum, object[] objects)
        {
            yield return new WaitForSeconds(_delayAppearDuration);
            
            _canvasGroup.gameObject.SetActive(true);
            _canvasGroup.alpha = 0;
            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1, _appearDuration).SetEase(_appearEase);
        }
        
        
    }
}