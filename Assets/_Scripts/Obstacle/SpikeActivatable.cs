using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Obstacle;
using UnityEngine;

public class SpikeActivatable : Activatable
{
    private void Start()
    {
        Inactive();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    protected override void Active()
    {
        gameObject.SetActive(true);
    }

    protected override void Inactive()
    {
        gameObject.SetActive(false);
    }
}
