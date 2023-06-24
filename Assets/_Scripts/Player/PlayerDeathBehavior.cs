using _Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathBehavior : MonoBehaviour
{
    [SerializeField] private int _fragmentCount = 10; // Number of fragments to create
    [SerializeField] private float _explosionForce = 10f; // Magnitude of the explosion force

    // Start is called before the first frame update
    void Start()
    {
        NormalStateMachine.OnKillPlayer += Explode;
    }
    
    private void Explode()
    {
        for (int i = 0; i < _fragmentCount; ++i)
        {
            var fleshFragment = Instantiate(ResourceManager.Instance.FleshCollectible, transform.position, Quaternion.identity);
            Rigidbody2D fragmentRigidbody = fleshFragment.GetComponent<Rigidbody2D>();

            // Generate a random direction in the upper hemisphere
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            randomDirection.y = Mathf.Abs(randomDirection.y); // Keep the direction in the upper hemisphere

            // Apply explosion force to the fragment
            fragmentRigidbody.AddForce(randomDirection * _explosionForce, ForceMode2D.Impulse);
        }

        
    }

}
