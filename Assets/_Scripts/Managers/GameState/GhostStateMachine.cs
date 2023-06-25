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
    public static Action OnKillPlayer;

    private List<MapManager.ActivatablePair> _currentActivatablePairs = new ();
    private List<MapManager.ActivatablePair> _lastActivatablePairs = new ();

    private PlayerGhostMovement _playerGhostMovement;
    private PlayerNormalMovement _playerNormalMovement;
    private bool _isWaitForInput = false; 
    private void Start()
    {
        _playerGhostMovement = DataManager.Instance.PlayerGhostMovement;
        _playerNormalMovement = DataManager.Instance.PlayerNormalMovement;
        
        OnWallCollide += NextCycle;
        OnRevivePlayer += RevivePlayer;
        
        AddToFunctionQueue(InitActivatable, StateEvent.OnEnter);
        AddToFunctionQueue(InitGhostPlayer, StateEvent.OnEnter);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isWaitForInput)
        {
            _playerGhostMovement.Unfreeze();
            _isWaitForInput = false;
        }
    }

    void InitGhostPlayer(GameStateEnum newState, object [] enterParameters)
    {
        _playerGhostMovement.gameObject.SetActive(true);
        _playerGhostMovement.Freeze();
        _playerGhostMovement.transform.position = _playerNormalMovement.transform.position;
        _isWaitForInput = true;
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

    private void RevivePlayer()
    {
        foreach (var pair in _currentActivatablePairs)
        { 
            pair.SpikeActivatable.SetActiveActivatable(false);
        }

        GameManager.Instance.SetToState(GameStateEnum.NormalState, null,  new []
        {
            _currentActivatablePairs, // swap current and last
            _lastActivatablePairs
        });
    }
    
}
