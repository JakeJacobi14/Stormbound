using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public string levelNum;
    public GameObject obstaclesPrefab; 
    // public int maxCharges;
    public int ThreeStarsCharges;
    public int TwoStarsCharges;
    public Vector2 StartingLoc;
    [HideInInspector] public int stars = 0;
}
