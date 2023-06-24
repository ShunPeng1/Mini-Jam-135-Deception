using _Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlesh : MonoBehaviour
{
    [SerializeField] private GameObject flesh;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnEnable()
    {
        for (int i = 0; i < 5; ++i)
        {
            Vector2 spawnpos = new Vector2(Random.Range(-12, 13), Random.Range(-8, 9));
            Instantiate(flesh, spawnpos, Quaternion.identity);
        }
    }
}
