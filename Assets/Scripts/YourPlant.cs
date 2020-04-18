using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public int Attention;
    public int Hunger;
    public int Thirst;
    public int Stimulation;
    public int Knowledge;
    public int Reassurance;
    public int Peace;

    public int day = 1;
    public int pointsToday = 0;
    public int freeTime = 1;

    private static YourPlant the_plant;
    public static YourPlant GetInstancePlant()
    {
        return the_plant;
    }

    public static string NewName()
    {
        var prefix = new string[] {"The Greater", "The Lesser", "Mr.", "Mrs.", "The Regal", "The Opaque",
            "The Oblique", "The Reticulated", "The Variegated", "The Online"};

        var first = new string[] {"Floridian", "Alaskan", "Californian", "Rigelian", "Venusian", "Jovian",
            "Sub - Alaskan", "West Virginian", "East Virginian", "Cape Breton", "Oily", "Dusky", "Fragrant",
            "Nasty", "Green", "Texan", "Inverted Texan", "Cosmic", "Second Stage", "Lazy", "Digital"};

        var second = new string[] {"Beach", "Ass", "Sports", "Cave", "Stinky", "Canyon", "Office", "Rubber",
            "Thoughtful", "Tufted", "Flanged", "Galvanic", "Fake", "Eggy", "Pabst", "Turbine", "Walking",
            "Singing", "Death", "Twisting"};

        var third = new string[] {"Palm", "Pansy", "Dandelion", "Potato", "Frond", "Willow", "Berry", "Cactus",
            "Vine", "Rose", "Oak", "Cucumber", "Carrot", "Fern", "Pear", "Lily", "Clover", "Stalk", "Tulip", "Weed"};

        string name = "";
        if(Random.value < 0.25) {
            name += prefix[Random.Range(0, prefix.Length)];
        }

        if(Random.value < 0.7) {
            if (name != "") name += " ";
            name += first[Random.Range(0, first.Length)];
        }

        if (Random.value < 0.8) {
            if (name != "") name += " ";
            name += second[Random.Range(0, second.Length)];
        }

        if (Random.value < 0.8) {
            if (name != "") name += " ";
            name += third[Random.Range(0, third.Length)];
        }

        if (name == "") return NewName();

        return name;
    }

    // Start is called before the first frame update
    void Start()
    {
        the_plant = this;

        plantTileCollection = (PlantTileCollection)FindObjectOfType<PlantTileCollection>();

        plantGrid = new PlantPart[plantGridSizeX, plantGridSizeY];
        plantPot = new PlantPart() { x = 8, y = 0, plantTile = plantTileCollection.plantTiles[0] };
        plantGrid[plantPot.x, plantPot.y] = plantPot;
        PlantPart pp = new PlantPart();

        plantPotTileMap.SetTile(new Vector3Int(plantPot.x, plantPot.y, 0), plantPot.plantTile.tile);

        // Generate name
        PlantName = NewName();
        Attention = Random.Range(-3, 4);
        Hunger = Random.Range(-3, 4);
        Thirst = Random.Range(-3, 4);
        Stimulation = Random.Range(-3, 4);
        Knowledge = Random.Range(-3, 4);
        Reassurance = Random.Range(-3, 4);
        Peace = Random.Range(-3, 4);
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

    public void ApplyInteration(ObjectInteraction interaction)
    {
        int points = 0;
        points += Attention * interaction.Attention;
        points += Hunger * interaction.Hunger;
        points += Thirst * interaction.Thirst;
        points += Stimulation * interaction.Stimulation;
        points += Knowledge * interaction.Knowledge;
        points += Reassurance * interaction.Reassurance;
        points += Peace * interaction.Peace;
        pointsToday += points;
        UseTime();
    }

    public int TimeOnDay(int day)
    {
        return System.Math.Min((day-1)/2+1, 8);
    }

    public void ModifyPlant(int points)
    {
        Debug.Log(points);
    }

    public void UseTime()
    {
        freeTime -= 1;
        if(freeTime == 0) {
            day += 1;
            ModifyPlant(pointsToday);
            pointsToday = 0;
            freeTime = TimeOnDay(day);
        }

        GameObject.Find("DayIndicator").GetComponent<Text>().text = day.ToString();
        GameObject.Find("TimeIndicator").GetComponent<Text>().text = freeTime.ToString();
    }
}
