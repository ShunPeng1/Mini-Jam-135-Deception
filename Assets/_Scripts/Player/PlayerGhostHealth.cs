using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerGhostHealth : MonoBehaviour
{
    [Header("Flesh Collectibles")]
    [SerializeField] private int _fleshCollectibleRequirement = 14;
    [SerializeField] private float _playerCheckRange = 1f;
    private int _fleshCollectibleCount = 0;


    [SerializeField] private float _maxHealthTimer = 1f;
    private float _currentHealthTimer;
    private int _lightEnteringCounter = 0;
    
    public bool IsHurt = false;

    
    // Start is called before the first frame update
    private void Update()
    {
        if (_lightEnteringCounter > 0)
        {
            _currentHealthTimer += Time.deltaTime;
            
            if (_currentHealthTimer >= _maxHealthTimer)
            {
                GhostStateMachine.OnKillGhost.Invoke();
                _lightEnteringCounter = 0;
            }
        }

        CheckPlayerRevival();
    }

    public void EnterLight()
    {
        _lightEnteringCounter++;
        IsHurt = true;
    }
    public void ExitLight()
    {
        _lightEnteringCounter--;
        if (_lightEnteringCounter == 0) IsHurt = false;
    }

    public void AddFlesh()
    {
        _fleshCollectibleCount++;
    }

    private void CheckPlayerRevival()
    {
        if(Vector3.Distance( DataManager.Instance.PlayerNormalMovement.transform.position, transform.position) > _playerCheckRange) return;

        if (_fleshCollectibleCount < _fleshCollectibleRequirement) return;
        
        GhostStateMachine.OnRevivePlayer.Invoke();
        _fleshCollectibleCount = 0;
        _lightEnteringCounter = 0;

        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _playerCheckRange);
    }
}
