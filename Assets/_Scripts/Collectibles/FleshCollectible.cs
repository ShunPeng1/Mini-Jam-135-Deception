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

    protected override void OnCollect(GameObject player) {
        
    }
}
