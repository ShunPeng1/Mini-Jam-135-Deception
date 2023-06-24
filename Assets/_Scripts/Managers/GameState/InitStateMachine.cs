using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using Collections.StateMachine;
using UnityEngine;


public class InitStateMachine : StateMachine<InitStateMachine,GameStateEnum>
{
    
    // Start is called before the first frame update
    void Awake()
    {
        AddToFunctionQueue(Initialize, StateEvent.OnEnter);
    }

    void Initialize(GameStateEnum oldState, object [] enterParameters)
    {
        DataManager.Instance.PlayerNormalMovement.enabled = false;
    }

    public void Update()
    {
        if (GameManager.Instance.CurrentStateMachine.MyStateEnum != GameStateEnum.InitState) return;
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.SetToState(GameStateEnum.NormalState);
        }
    }
    
    
}
