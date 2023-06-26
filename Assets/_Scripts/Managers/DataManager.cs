using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    public PlayerNormalMovement PlayerNormalMovement;
    public PlayerGhostMovement PlayerGhostMovement;

    public Action OnScrollChange; 
    
    public int ScrollCount = 0;

    public void AddScroll()
    {
        ScrollCount++;
        OnScrollChange.Invoke();
    }
}
