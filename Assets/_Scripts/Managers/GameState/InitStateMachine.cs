using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using Collections.StateMachine;
using UnityEngine;


public class InitStateMachine : StateMachine<InitStateMachine,GameStateEnum>
{
    private PlayerNormalMovement _playerNormalMovement;
    
    // Start is called before the first frame update
    void Awake()
    {
        AddToFunctionQueue(Initialize, StateEvent.OnEnter);
        AddToFunctionQueue(EndInitialize, StateEvent.OnExit);
    }

    void Initialize(GameStateEnum oldState, object [] enterParameters)
    {
        _playerNormalMovement = FindObjectOfType<PlayerNormalMovement>();
        _playerNormalMovement.enabled = false;
    }

    public void Update()
    {
        if (GameManager.Instance.CurrentStateMachine.MyStateEnum != GameStateEnum.InitState) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.SetToState(GameStateEnum.NormalState);
        }
    }
    
    void EndInitialize(GameStateEnum newState, object [] enterParameters)
    {
        _playerNormalMovement.enabled = true;
    }
    
}
