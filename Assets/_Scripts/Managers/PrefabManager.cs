using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;


public class PrefabManager : SingletonMonoBehaviour<PrefabManager>
{
    [SerializeField] private List<FleshCollectible> _fleshCollectibles;

    public FleshCollectible GetRandomFlesh()
    {
        return _fleshCollectibles[Random.Range(0, _fleshCollectibles.Count)];
    }
}
