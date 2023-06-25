using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhostVisual : MonoBehaviour
{
    private PlayerGhostMovement _playerGhostMovement;
    private PlayerGhostHealth _playerGhostHealth;
    private Animator _animator;

    [Header("Animation")]
    private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
    private static readonly int IsDead = Animator.StringToHash("IsDead");
    private static readonly int IsHurt = Animator.StringToHash("IsHurt");
    
    
    // Start is called before the first frame update
    void Awake()
    {
        _playerGhostMovement = GetComponent<PlayerGhostMovement>();
        _playerGhostHealth = GetComponent<PlayerGhostHealth>();
        _animator = GetComponent<Animator>();

        GhostStateMachine.OnKillPlayer += DeadAnimation;
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementAnimation();   
    }

    public void Hurt()
    {
        _animator.SetTrigger(IsHurt);
    }
    
    private void MovementAnimation()
    {
        _animator.SetFloat(RunSpeed, Mathf.Abs(_playerGhostMovement._moveInput.x));
        _animator.SetBool(IsHurt, _playerGhostHealth.IsHurt);
    }

    private void DeadAnimation()
    {
        _animator.SetBool(IsDead, true);
    }
    
}
