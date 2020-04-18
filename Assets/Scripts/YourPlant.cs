using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class YourPlant : MonoBehaviour
{
    public string PlantName;
    
    [System.Serializable]
    public class PlantPart
    {
        public int x;
        public int y;

        public PlantTiles plantTile = null;
    }
    
    public int plantGridSizeX = 17;
    public int plantGridSizeY = 9;

    public PlantPart[,] plantGrid;
    private PlantPart plantPot;

    public UnityEngine.Tilemaps.Tilemap plantPotTileMap = null;
    public UnityEngine.Tilemaps.Tilemap plantBranchTileMap = null;
    public UnityEngine.Tilemaps.Tilemap plantFoliageTileMap = null;

    private PlantTileCollection plantTileCollection = null;

    // Start is called before the first frame update
    void Start()
    {
        plantTileCollection = (PlantTileCollection)FindObjectOfType<PlantTileCollection>();

        plantGrid = new PlantPart[plantGridSizeX, plantGridSizeY];
        plantPot = new PlantPart() { x = 8, y = 0, plantTile = plantTileCollection.plantTiles[0] };
        plantGrid[plantPot.x, plantPot.y] = plantPot;
        PlantPart pp = new PlantPart();

        plantPotTileMap.SetTile(new Vector3Int(plantPot.x, plantPot.y, 0), plantPot.plantTile.tile);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GrowPlant()
    {

    }

    public void ShrinkPlant()
    {

    }
}
