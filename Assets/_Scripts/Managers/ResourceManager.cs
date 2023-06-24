using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;
using Random = UnityEngine.Random;


public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
{
    [SerializeField] private Sprite [] _fleshSprites;
    public FleshCollectible FleshCollectible;
    private RandomBag<Sprite> _fleshSpriteBag;

    private void Awake()
    {
        _fleshSpriteBag = new RandomBag<Sprite>(_fleshSprites, 1);
    }

    public Sprite GetRandomFleshSprite()
    {
        return _fleshSpriteBag.PopRandomItem();
    }
}
