using System.Collections;
using System.Collections.Generic;
using _Scripts.Collectible;
using DG.Tweening;
using UnityEngine;

public class ScrollCollectible : Collectible
{
    [SerializeField] private int _point;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _collectSoundEffect;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private float _yIdleFluctuation = 1f;
    [SerializeField] private float _fluctuationDuration = 1f;
    private Tween _fluctuation;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        
        _fluctuation = transform.DOMoveY(transform.position.y + _yIdleFluctuation, _fluctuationDuration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }

    protected override void OnCollect(GameObject player)
    {
        DataManager.Instance.AddScroll();
        _fluctuation.Kill();
        this.DestroyCollectible();
    }
    
}
