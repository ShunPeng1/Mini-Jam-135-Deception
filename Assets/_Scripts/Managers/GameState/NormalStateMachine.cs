using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using Collections.StateMachine;
using UnityEngine;using UnityUtilities;

public class NormalStateMachine : StateMachine<NormalStateMachine,GameStateEnum>
{
    public static Action OnWallCollide;
    public static Action OnKillPlayer;
    
    private List<MapManager.ActivatablePair> _currentActivatablePairs = new ();
    private List<MapManager.ActivatablePair> _lastActivatablePairs = new ();

    private void Start()
    {
        OnWallCollide += NextCycle;
        OnKillPlayer += KillPlayer;
        NextCycle();
        
        AddToFunctionQueue(InitActivatable, StateEvent.OnEnter);
        AddToFunctionQueue(InitNormalPlayer, StateEvent.OnEnter);

    }

    void InitNormalPlayer(GameStateEnum newState, object [] enterParameters)
    {
        DataManager.Instance.PlayerNormalMovement.gameObject.SetActive(true);
        DataManager.Instance.PlayerNormalMovement.enabled = true;
    }

    void InitActivatable(GameStateEnum lastState, object[] enterParameters)
    {
        if (enterParameters == null) return;

        _lastActivatablePairs = enterParameters[0] as List<MapManager.ActivatablePair>;
        _currentActivatablePairs = enterParameters[1] as List<MapManager.ActivatablePair>;
    }

    private void NextCycle()
    {
        var nextActivatablePairs = MapManager.Instance.GenerateNextActivatable();

        foreach (var pair in _lastActivatablePairs)
        {
            if(!_currentActivatablePairs.Contains(pair)) pair.SpikeActivatable.SetActiveActivatable(false);
            if(!nextActivatablePairs.Contains(pair)) pair.LightActivatable.SetActiveActivatable(false);
        }

        foreach (var pair in nextActivatablePairs)
        {
            pair.LightActivatable.SetActiveActivatable(true);
        }
        
        foreach (var pair in _currentActivatablePairs)
        {
            if(!nextActivatablePairs.Contains(pair)) pair.LightActivatable.SetActiveActivatable(false);
            pair.SpikeActivatable.SetActiveActivatable(true);
        }

        _lastActivatablePairs = _currentActivatablePairs;
        _currentActivatablePairs = nextActivatablePairs;
        
    }

    public void KillPlayer()
    {
        DataManager.Instance.PlayerNormalMovement.enabled = false;
        DataManager.Instance.PlayerNormalMovement.gameObject.SetActive(false);
        
        
        foreach (var pair in _currentActivatablePairs)
        { 
            pair.LightActivatable.SetActiveActivatable(false);
        }
        
        GameManager.Instance.SetToState(GameStateEnum.GhostState, null,  new []
        {
            _currentActivatablePairs, // swap last and current
            _lastActivatablePairs
        });
    }
}
