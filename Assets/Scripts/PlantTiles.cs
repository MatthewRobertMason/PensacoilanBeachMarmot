using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class PlantTiles
{
    public string description = null;

    public bool isFoliage = false;

    public bool pointsUp = false;
    public bool pointsRight = false;
    public bool pointsDown = false;
    public bool pointsLeft = false;
    
    public bool isPlantPot = false;
     
    public UnityEngine.Tilemaps.Tile tile = null;
}
