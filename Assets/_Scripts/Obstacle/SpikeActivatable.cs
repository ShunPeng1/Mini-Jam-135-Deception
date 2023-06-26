using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.Obstacle;
using DG.Tweening;
using UnityEngine;

public class SpikeActivatable : Activatable
{
    [SerializeField] private LayerMask _triggerLayerMask;
    [SerializeField] private float _goUpDuration = 0.5f, _goDownDuration = 0.5f;
    [SerializeField] private Ease _goUpEase, _goDownEase;
    

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

        transform.localScale = new Vector3(1,0,0);
        transform.DOScaleY(1, _goUpDuration).SetEase(_goUpEase);

    }

    protected override void Inactive()
    {
        transform.localScale = new Vector3(1,1,0);
        transform.DOScaleY(0, _goDownDuration).SetEase(_goDownEase)
            .OnComplete(()=> gameObject.SetActive(false));
    }
}
