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
    
    private int _scrollCount = 0;

    public void AddScroll()
    {
        _scrollCount++;
        OnScrollChange.Invoke();
    }
}
