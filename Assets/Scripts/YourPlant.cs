using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourPlant : MonoBehaviour
{
    public string PlantName;
    
    [System.Serializable]
    public class PlantPart
    {
        public int x;
        public int y;
    }

    public int plantGridSizeX = 19;
    public int plantGridSizeY = 10;

    public PlantPart[,] plantGrid;
    public PlantPart plantPot;

    // Start is called before the first frame update
    void Start()
    {
        plantGrid = new PlantPart[plantGridSizeX, plantGridSizeY];
        plantPot = new PlantPart() { x = plantGridSizeX / 2, y = 0 };
        plantGrid[plantPot.x, plantPot.y] = plantPot;
        PlantPart pp = new PlantPart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
