using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Obstacle;
using UnityEngine;
using UnityUtilities;
using Random = UnityEngine.Random;

public class MapManager : SingletonMonoBehaviour<MapManager>
{
    [Serializable, Tooltip("The group that guarantee there must be at least 1 activatable inactive")]
    class ActivatableRegion 
    {
        public ActivatablePair [] Activatables;
    }

    [Serializable]
    public class ActivatablePair
    {
        public LightActivatable LightActivatable;
        public SpikeActivatable SpikeActivatable;
    }

    [Serializable]
    class ActivatableSampleFrequency
    {
        public int ActivatableCount; 
        [SerializeField, Tooltip("The amount of sample")] private AnimationCurve _sampleSize;

        public float GetSampleSize(float cycle)
        {
            return _sampleSize.Evaluate(cycle);
        }
    }
    
    [SerializeField] private List<ActivatableRegion> _activatableRegions;
    [SerializeField] private List<ActivatableSampleFrequency> _activatableSampleFrequencies;
    private List<ActivatablePair> _allActivatablePairs = new List<ActivatablePair>();
    
    private void OnValidate()
    {
        foreach (var activatableRegion in _activatableRegions)
        {
            if (activatableRegion != null)
            {
                foreach (var activatablePair in activatableRegion.Activatables)
                {
                    _allActivatablePairs.Add(activatablePair);
                }
            }
        }
    }
    
    public List<ActivatablePair> GenerateNextActivatable(int cycle)
    {
        List<ActivatablePair> result = new List<ActivatablePair>();
        int willActivatableCount = GenerateActivatableCount(cycle);
        int wontActivatableCount = _allActivatablePairs.Count - willActivatableCount;

        
        List<RandomBag<ActivatablePair>> randomBags = _activatableRegions.Select(region => new RandomBag<ActivatablePair>(region.Activatables, 1)).ToList();

        int currentBagIndex = 0;
        while (willActivatableCount != 0)
        {
            while (randomBags[currentBagIndex].RemainderCount == 1)
            {
                currentBagIndex ++;
                wontActivatableCount--;
            }
            
            // Get an item in bag?
            float pBinomial = (float) willActivatableCount/(willActivatableCount + wontActivatableCount);
            float randomPdf = Random.Range(0f, 1f);

            var activatablePair = randomBags[currentBagIndex].PopRandomItem();
            if (pBinomial < randomPdf)
            {
                result.Add(activatablePair);
                willActivatableCount --;
            }
            else
            {
                wontActivatableCount--;
            }
        }

        return result;
    }

    public int GenerateActivatableCount(int cycle)
    {
        float totalSample = _activatableSampleFrequencies.Sum(activatableSampleFrequency => activatableSampleFrequency.GetSampleSize(cycle));
        float randomCdf = Random.Range(0f, 1f);
        
        foreach (var activatableSampleFrequency in _activatableSampleFrequencies)
        {
            float currentCdf = (float) activatableSampleFrequency.GetSampleSize(cycle) / totalSample;
            if (randomCdf < currentCdf)
            {
                return activatableSampleFrequency.ActivatableCount;
            }
            else
            {
                randomCdf -= currentCdf;
            }
        }

        return 0;
    }
    
}
