using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.Collectible
{

    [RequireComponent(typeof(Collider2D))]
    public abstract class Collectible : MonoBehaviour
    {
        [SerializeField] private string _collectorTag;
        private bool _isCollected = false;
        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            if (_isCollected) return;
            
            if (col.CompareTag(_collectorTag))
            {
                OnCollect(col.gameObject);
                _isCollected = true;
            } 
        }

        protected abstract void OnCollect(GameObject player);

        public virtual void DestroyCollectible()
        {
            Destroy(gameObject);
        }
    }
}