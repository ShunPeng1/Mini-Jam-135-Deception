using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalVisual : MonoBehaviour
{
    private PlayerNormalMovement _playerNormalMovement;
    private Animator _animator;

    [Header("Animation")]
    private static readonly int Jumping = Animator.StringToHash("IsJumping");
    private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
    
    // Start is called before the first frame update
    void Awake()
    {
        _playerNormalMovement = GetComponent<PlayerNormalMovement>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        VisualizeAnimation();   
    }
    
    private void VisualizeAnimation()
    {
        if(_playerNormalMovement.LastPressedJumpTime == 0) _animator.SetTrigger(Jumping);
        _animator.SetFloat(RunSpeed, Mathf.Abs(_playerNormalMovement._moveInput.x));
    }
}
