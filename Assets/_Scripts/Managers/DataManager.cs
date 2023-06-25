using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    public PlayerNormalMovement PlayerNormalMovement;
    public PlayerGhostMovement PlayerGhostMovement;

    public int FleshCollectibleCount = 0;
    public int FleshCollectibleRequirement = 0;
    
    
}
