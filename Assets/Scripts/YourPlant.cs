using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public PlantTiles foliageTile = null;
        public bool foliage = false;
        public bool plantPot = false;
        
        public bool pointsUp = false;
        public bool pointsRight = false;
        public bool pointsDown = false;
        public bool pointsLeft = false;
        public bool pointsFoliage = false;

        public PlantPart parent = null;
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
    public HashSet<string> actionsToday = new HashSet<string>();

    private static YourPlant the_plant;
    public static YourPlant GetInstancePlant()
    {
        return the_plant;
    }

    public GameObject SplashPrefab;

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
        plantPot = new PlantPart() { x = 8, y = 0, plantTile = plantTileCollection.plantTiles[0], pointsUp = true, plantPot = true };
        
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
        HashSet<PlantPart> visitedPlaces = new HashSet<PlantPart>();
        Queue<PlantPart> fringe = new Queue<PlantPart>();

        List<PlantPart> potentialGrowths = new List<PlantPart>();
        List<PlantPart> potentialFoliage = new List<PlantPart>();

        PlantPart current = null;
        fringe.Enqueue(plantPot);

        while (fringe.Count > 0)
        {
            Debug.Log("Queue Size:");
            current = fringe.Dequeue();

            // Up
            if (current.pointsUp)
            {
                if ((current.plantPot) && (FreeSpace(current.x, current.y + 1)))
                    potentialGrowths.Add(new PlantPart() { x = current.x, y = current.y + 1, pointsDown = true, parent = current });
                else if (!visitedPlaces.Contains(plantGrid[current.x, current.y + 1]))
                    fringe.Enqueue(plantGrid[current.x, current.y + 1]);
            }
            if ((!current.plantPot) && (FreeSpace(current.x, current.y + 1)))
                potentialGrowths.Add(new PlantPart() { x = current.x, y = current.y + 1, pointsDown = true, parent = current });

            // Right
            if ((current.pointsRight) && (!visitedPlaces.Contains(plantGrid[current.x + 1, current.y])))
                fringe.Enqueue(plantGrid[current.x + 1, current.y]);
            if ((!current.plantPot) && (FreeSpace(current.x + 1, current.y)))
                potentialGrowths.Add(new PlantPart() { x = current.x + 1, y = current.y, pointsLeft = true, parent = current });

            // Down
            if ((current.pointsDown) && (!visitedPlaces.Contains(plantGrid[current.x, current.y - 1])))
                fringe.Enqueue(plantGrid[current.x, current.y - 1]);
            if ((!current.plantPot) && (FreeSpace(current.x, current.y - 1)))
                potentialGrowths.Add(new PlantPart() { x = current.x, y = current.y - 1, pointsUp = true, parent = current });

            // Left
            if ((current.pointsLeft) && (!visitedPlaces.Contains(plantGrid[current.x - 1, current.y])))
                fringe.Enqueue(plantGrid[current.x - 1, current.y]);
            if ((!current.plantPot) && (FreeSpace(current.x - 1, current.y)))
                potentialGrowths.Add(new PlantPart() { x = current.x - 1, y = current.y, pointsRight = true, parent = current });

            // Foliage
            if ((!current.plantPot) && (!current.pointsFoliage))
                potentialFoliage.Add(new PlantPart() { x = current.x, y = current.y, parent = current, foliage = true });

            visitedPlaces.Add(current);
        }

        PlantPart newPart = null;

        int rand = Random.Range(0, potentialGrowths.Count + potentialFoliage.Count);

        if (rand >= potentialGrowths.Count)
        {
            rand -= potentialFoliage.Count;
            newPart = potentialFoliage[rand];
        }
        else
        {
            newPart = potentialGrowths[rand];
        }

        if (newPart.pointsDown)
            newPart.parent.pointsUp = true;
        if (newPart.pointsLeft)
            newPart.parent.pointsRight = true;
        if (newPart.pointsUp)
            newPart.parent.pointsDown = true;
        if (newPart.pointsRight)
            newPart.parent.pointsLeft = true;

        UpdateTile(newPart);
        plantBranchTileMap.SetTile(new Vector3Int(newPart.x, newPart.y, 0), newPart.plantTile.tile);
        if (newPart.foliage)
            plantFoliageTileMap.SetTile(new Vector3Int(newPart.x, newPart.y, 0), newPart.foliageTile.tile);

        if (!newPart.parent.plantPot)
        {
            UpdateTile(newPart.parent);
            plantBranchTileMap.SetTile(new Vector3Int(newPart.parent.x, newPart.parent.y, 0), newPart.parent.plantTile.tile);
            if (newPart.parent.foliage)
                plantFoliageTileMap.SetTile(new Vector3Int(newPart.parent.x, newPart.parent.y, 0), newPart.parent.foliageTile.tile);
        }
        
        plantGrid[newPart.x, newPart.y] = newPart;
    }
    
    public void UpdateTile(PlantPart part)
    {
        List<PlantTiles> potentialBranchTiles = null;
        List<PlantTiles> potentialFoliageTiles = null;

        if (part.foliage == false)
        {
            potentialBranchTiles = plantTileCollection.plantTiles.Where(p =>
                (
                    p.pointsUp
                    || p.pointsRight
                    || p.pointsDown
                    || p.pointsLeft
                )
                && !p.isPlantPot
                && !p.isFoliage
            ).ToList();
        }

        if ((part.foliageTile == null) && (part.foliage == false))
        {
            potentialFoliageTiles = plantTileCollection.plantTiles.Where(p =>
                p.isFoliage == part.foliage
            ).ToList();
        }

        if (potentialBranchTiles.Count > 0)
        {
            int rand = Random.Range(0, potentialBranchTiles.Count);
            part.plantTile = potentialBranchTiles[rand];
        }

    }

    public void ShrinkPlant()
    {

    }

    private bool FreeSpace(int x, int y)
    {
        return (x >= 0) && (y >= 0) && (x < plantGridSizeX) && (y < plantGridSizeY) && plantGrid[x, y] == null;
    }

    public void ApplyInteration(ObjectInteraction interaction)
    {
        int points = 0;
        actionsToday.Add(interaction.InteractionDescription);
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
        return System.Math.Min(day, 8);
    }

    public void ModifyPlant(int points)
    {
        if(points > 0) {
            for(int ii = 0; ii < points / 2; ii++) {
                GrowPlant();
            }
        } else {
            for (int ii = 0; ii < -points / 2; ii++) {
                ShrinkPlant();
            }
        }
    }

    public void UseTime()
    {
        freeTime -= 1;
        if(freeTime == 0) {
            day += 1;
            ModifyPlant(pointsToday);
            pointsToday = 0;
            freeTime = TimeOnDay(day);
            actionsToday.Clear();
            var splash = Instantiate(SplashPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            splash.GetComponent<Splash>().SetDay(day);
        }
        
        GameObject.Find("DayIndicator").GetComponent<Text>().text = day.ToString();
        GameObject.Find("TimeIndicator").GetComponent<Text>().text = freeTime.ToString();
    }


}
