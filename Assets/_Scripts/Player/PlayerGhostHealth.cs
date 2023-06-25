using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerGhostHealth : MonoBehaviour
{
    
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
                GhostStateMachine.OnKillPlayer.Invoke();
            }
        }
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
}
