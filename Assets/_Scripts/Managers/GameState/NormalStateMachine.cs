using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using Collections.StateMachine;
using UnityEngine;using UnityUtilities;

public class NormalStateMachine : StateMachine<NormalStateMachine,GameStateEnum>
{
    // #region Singleton
    //
    // static NormalStateMachine _instance;
    // public static NormalStateMachine Instance
    // {
    //     get
    //     {
    //         if (_instance == null)
    //         {
    //             var instances = FindObjectsOfType<NormalStateMachine>();
    //         }
    //         return _instance;
    //     }
    // }
    // #endregion
    //
    public static Action OnWallCollide;
    
    private int _cycle = 0;
    private List<MapManager.ActivatablePair> _currentActivatablePairs = new ();
    private List<MapManager.ActivatablePair> _lastActivatablePairs = new ();

    private void Start()
    {
        OnWallCollide += NextCycle;
        NextCycle();
    }

    private void NextCycle()
    {
        var nextActivatablePairs = MapManager.Instance.GenerateNextActivatable(_cycle);

        foreach (var pair in _lastActivatablePairs)
        {
            if(!_currentActivatablePairs.Contains(pair)) pair.SpikeActivatable.SetActiveActivatable(false);
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
        _cycle++;
    }
    
}
