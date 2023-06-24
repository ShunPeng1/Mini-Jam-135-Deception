using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Collectibles;
using _Scripts.Managers;
using Collections.StateMachine;
using UnityEngine;

public class GhostStateMachine : StateMachine<GhostStateMachine, GameStateEnum>
{
    public static Action OnWallCollide;
    public static Action OnRevivePlayer;
    
    private List<MapManager.ActivatablePair> _currentActivatablePairs = new ();
    private List<MapManager.ActivatablePair> _lastActivatablePairs = new ();

    private void Start()
    {
        OnWallCollide += NextCycle;
        AddToFunctionQueue(InitActivatable, StateEvent.OnEnter);
        AddToFunctionQueue(InitGhostPlayer, StateEvent.OnEnter);
    }

    void InitGhostPlayer(GameStateEnum newState, object [] enterParameters)
    {
        DataManager.Instance.PlayerGhostMovement.gameObject.SetActive(true);
        DataManager.Instance.PlayerGhostMovement.enabled = true;

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
            if(!_currentActivatablePairs.Contains(pair)) pair.LightActivatable.SetActiveActivatable(false);
        }

        foreach (var pair in nextActivatablePairs)
        {
            pair.SpikeActivatable.SetActiveActivatable(true);
        }
        
        foreach (var pair in _currentActivatablePairs)
        {
            if(!nextActivatablePairs.Contains(pair)) pair.SpikeActivatable.SetActiveActivatable(false);
            pair.LightActivatable.SetActiveActivatable(true);
        }

        _lastActivatablePairs = _currentActivatablePairs;
        _currentActivatablePairs = nextActivatablePairs;
    }
    
    public void RevivePlayer()
    {
        
        DataManager.Instance.FleshCollectibleCount = 0;
        
        GameManager.Instance.SetToState(GameStateEnum.NormalState, null,  new []
        {
            _currentActivatablePairs, // swap current and last
            _lastActivatablePairs
        });
    }
}
