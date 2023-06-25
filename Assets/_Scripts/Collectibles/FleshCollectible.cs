using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Collectible;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FleshCollectible : Collectible
{
    //Constant
    const string collectAnim = "Collect";

    [SerializeField] private int _point;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _collectSoundEffect;
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private CircleCollider2D _circleCollider2D;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private LayerMask _ignoreLayer; // The layer to ignore collisions with
    [SerializeField] private float _ignoreCollisionDuration = 1f; // The duration to ignore collisions for

    private bool _isStuck;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = ResourceManager.Instance.GetRandomFleshSprite();

        StartCoroutine(nameof(IgnoreLayerForTime));
    }

    protected override void OnCollect(GameObject player) {
        player.GetComponent<PlayerGhostHealth>().AddFlesh();
        this.DestroyCollectible();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isStuck) return;
        // Attach this to the collided object
        
        _isStuck = true;
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        _circleCollider2D.enabled = false;
    }

    private IEnumerator IgnoreLayerForTime()
    {
        int currentLayer = this.gameObject.layer;
        
        // Ignore collisions with the specified layer
        
        for (int i = 0; i < 32; i++)
        {
            if (((1 << i) & _ignoreLayer.value) != 0)
            {
                Physics2D.IgnoreLayerCollision(currentLayer, i, true);
            }
        }
        yield return new WaitForSeconds(_ignoreCollisionDuration);

        // Restore collision detection with the specified layer
        for (int i = 0; i < 32; i++)
        {
            if (((1 << i) & _ignoreLayer.value) != 0)
            {
                Physics2D.IgnoreLayerCollision(currentLayer, i, false);
            }
        }
    }
}
