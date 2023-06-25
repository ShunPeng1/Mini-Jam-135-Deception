using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.Obstacle;
using UnityEngine;

public class SpikeActivatable : Activatable
{
    [SerializeField] private LayerMask _triggerLayerMask;
    
    private void Start()
    {
        Inactive();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggerLayerMask == (_triggerLayerMask | (1 << other.gameObject.layer)))
        {
            if(GameManager.Instance.CurrentStateMachine.MyStateEnum == GameStateEnum.NormalState) 
                NormalStateMachine.OnKillPlayer.Invoke();
        }
    }

    protected override void Active()
    {
        gameObject.SetActive(true);
    }

    protected override void Inactive()
    {
        gameObject.SetActive(false);
    }
}
