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
    
    [Header("Audio")]
    [SerializeField] private AudioClip _jumpSfx;

    // Start is called before the first frame update
    void Awake()
    {
        _playerGhostMovement = GetComponent<PlayerGhostMovement>();
        _playerGhostHealth = GetComponent<PlayerGhostHealth>();
        _animator = GetComponent<Animator>();

    }

    private void Start()
    {
        GhostStateMachine.OnKillGhost += DeadAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        MovementAnimation();
        HurtAnimation();
    }

    public void HurtAnimation()
    {
        _animator.SetBool(IsHurt, _playerGhostHealth.IsHurt);
    }
    
    private void MovementAnimation()
    {
        _animator.SetFloat(RunSpeed, Mathf.Abs(_playerGhostMovement._moveInput.x));
        _animator.SetBool(IsHurt, _playerGhostHealth.IsHurt);
        
        if (_playerGhostMovement.LastPressedJumpTime == 0) 
            SoundManager.Instance.PlaySound(_jumpSfx);
    }

    private void DeadAnimation()
    {
        _animator.SetBool(IsDead, true);
    }

    public void DestroyAnimation()
    {
        Destroy(gameObject);        
    }

}
