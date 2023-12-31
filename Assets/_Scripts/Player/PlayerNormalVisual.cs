using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerNormalVisual : MonoBehaviour
{
    private PlayerNormalMovement _playerNormalMovement;
    private Animator _animator;

    [Header("Animation")]
    private static readonly int Jumping = Animator.StringToHash("IsJumping");
    private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
    private static readonly int IsDead = Animator.StringToHash("IsDead");

    [Header("Dead Animation")]
    [SerializeField] private int _fragmentCount = 10; // Number of fragments to create
    [SerializeField] private float _explosionForce = 15f; // Magnitude of the explosion force
    [SerializeField] private Vector3 _explosionOffsetPosition = Vector3.up;
    
    [Header("Audio")]
    [SerializeField] private AudioClip _jumpSfx;

    
    // Start is called before the first frame update
    void Awake()
    {
        _playerNormalMovement = GetComponent<PlayerNormalMovement>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        NormalStateMachine.OnKillPlayer += DeadAnimation;
        GhostStateMachine.OnRevivePlayer += ReviveAnimation;
    }

    // Update is called once per frame
    void Update()
    {
        MovementAnimation();   
    }
    
    private void MovementAnimation()
    {
        if (_playerNormalMovement.LastPressedJumpTime == 0)
        {
            _animator.SetTrigger(Jumping);
            SoundManager.Instance.PlaySound(_jumpSfx);
        }
        _animator.SetFloat(RunSpeed, Mathf.Abs(_playerNormalMovement.MoveInput.x));
        
    }

    private void DeadAnimation()
    {
        _animator.SetBool(IsDead, true);
    }
    
    private void ReviveAnimation()
    {
        _animator.SetBool(IsDead, false);
    }

    private void Explode()
    {
        for (int i = 0; i < _fragmentCount; ++i)
        {
            var fleshFragment = Instantiate(ResourceManager.Instance.FleshCollectible, transform.position + _explosionOffsetPosition, Quaternion.identity);
            Rigidbody2D fragmentRigidbody = fleshFragment.GetComponent<Rigidbody2D>();

            // Generate a random direction in the upper hemisphere
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            randomDirection.y = Mathf.Abs(randomDirection.y); // Keep the direction in the upper hemisphere

            // Apply explosion force to the fragment
            fragmentRigidbody.AddForce(randomDirection * _explosionForce, ForceMode2D.Impulse);
        }
    }
    
}
