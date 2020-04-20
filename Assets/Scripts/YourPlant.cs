using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


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

    public int day;
    public int growth;
    public int pointsToday = 0;
    public int freeTime = 1;
    public HashSet<string> actionsToday = new HashSet<string>();

    private static YourPlant the_plant;
    public static YourPlant GetInstancePlant()
    {
        return the_plant;
    }

    public GameObject SplashPrefab;
    public GameObject DropPrefab;
    public GameObject firstLetter;

    private AudioManager audioManager;

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
        day = Settings.TotalDays;
        growth = 0;
        

        plantTileCollection = (PlantTileCollection)FindObjectOfType<PlantTileCollection>();

        plantGrid = new PlantPart[plantGridSizeX, plantGridSizeY];
        plantPot = new PlantPart() { x = 8, y = 0, plantTile = plantTileCollection.plantTiles[0], pointsUp = true, plantPot = true };
        
        plantGrid[plantPot.x, plantPot.y] = plantPot;
        PlantPart pp = new PlantPart();

        plantPotTileMap.SetTile(new Vector3Int(plantPot.x, plantPot.y, 0), plantPot.plantTile.tile);

        // Generate name
        PlantName = NewName();
        while (true) {
            Attention = Random.Range(-3, 4);
            Hunger = Random.Range(-3, 4);
            Thirst = Random.Range(-3, 4);
            Stimulation = Random.Range(-3, 4);
            Knowledge = Random.Range(-3, 4);
            Reassurance = Random.Range(-3, 4);
            Peace = Random.Range(-3, 4);

            if (getHintsAt(3).Count >= 2) break;
        }

        firstLetter.GetComponent<Letter>().letterText = GenerateStartingLetter();

        GrowPlant();
        GrowPlant();
        GrowPlant();
        GrowPlant();
        growth = GrowPlant();
        UpdateHUD();

        audioManager = FindObjectOfType<AudioManager>();

        RandomPlantPot();
    }

    // Update is called once per frame

    float growDelay = 0;

    void Update()
    {
        if (day == 0)
        {
            audioManager.PlayGroodEndMusic();
            SceneManager.LoadScene("OkEnd");
        }

        if (freeTime == 0) {
            InteractiveObject.popupOpen = true;
            if (growDelay <= 0) {

                if (pointsToday >= 2) {
                    growth = GrowPlant();
                    if (growth >= Settings.TotalGrowth)
                    {
                        audioManager.PlayGoodEndMusic();
                        SceneManager.LoadScene("GoodEnd");
                    }
                    UpdateHUD();
                    pointsToday -= 2;
                    growDelay = 0.6f;
                } else if (pointsToday <= -2) {
                    if (!ShrinkPlant())
                    {
                        audioManager.PlayBadEndMusic();
                        SceneManager.LoadScene("BadEnd");
                    }
                    growth--;
                    pointsToday += 2;
                    growDelay = 0.6f;
                } else {
                    pointsToday = 0;
                    growDelay = 2f;
                }

                if (pointsToday == 0) StartDay();
            } else {
                growDelay -= Time.deltaTime;
            }
        } else {
            growDelay = 0.6f;
        }
    }

    public void GrowPlantButton()
    {
        GrowPlant();
    }

    public void ShrinkPlantButton()
    {
        ShrinkPlant();
    }

    public int GrowPlant()
    {
        HashSet<PlantPart> visitedPlaces = new HashSet<PlantPart>();
        Queue<PlantPart> fringe = new Queue<PlantPart>();

        List<PlantPart> potentialGrowths = new List<PlantPart>();
        List<PlantPart> potentialFoliage = new List<PlantPart>();

        PlantPart current = null;
        fringe.Enqueue(plantPot);

        while (fringe.Count > 0)
        {
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
            if ((!current.plantPot) && (!current.foliage))
            {
                potentialFoliage.Add(current);
            }

            visitedPlaces.Add(current);
        }

        PlantPart newPart = null;

        int rand = Random.Range(0, potentialGrowths.Count + potentialFoliage.Count);
        bool foliageWork = false;

        if (rand >= potentialGrowths.Count)
        {
            rand -= potentialGrowths.Count;
            newPart = potentialFoliage[rand];
            newPart.foliage = true;
            newPart.pointsFoliage = true;
            foliageWork = true;
        }
        else
        {
            newPart = potentialGrowths[rand];
        }

        if (!foliageWork)
        {
            if ((newPart.pointsDown) && (!newPart.parent.plantPot))
                newPart.parent.pointsUp = true;
            if (newPart.pointsLeft)
                newPart.parent.pointsRight = true;
            if (newPart.pointsUp)
                newPart.parent.pointsDown = true;
            if (newPart.pointsRight)
                newPart.parent.pointsLeft = true;
        }

        UpdateTile(newPart);
            plantBranchTileMap.SetTile(new Vector3Int(newPart.x, newPart.y, 0), newPart.plantTile.tile);
        if (newPart.foliageTile != null)
            plantFoliageTileMap.SetTile(new Vector3Int(newPart.x, newPart.y, 0), newPart.foliageTile.tile);

        if (!newPart.parent.plantPot)
        {
            UpdateTile(newPart.parent);
                plantBranchTileMap.SetTile(new Vector3Int(newPart.parent.x, newPart.parent.y, 0), newPart.parent.plantTile.tile);
            if (newPart.parent.foliageTile != null)
                plantFoliageTileMap.SetTile(new Vector3Int(newPart.parent.x, newPart.parent.y, 0), newPart.parent.foliageTile.tile);
        }
        
        plantGrid[newPart.x, newPart.y] = newPart;

        int size = 0;

        for (int xx = 0; xx < plantGridSizeX; xx++)
        {
            for (int yy = 0; yy < plantGridSizeY; yy++)
            {
                if (plantGrid[xx,yy] != null)
                {
                    if (plantGrid[xx, yy].foliage)
                        size += 2;
                    else
                        size += 1;
                }
            }
        }

        return size;
    }

    public void RandomPlantPot()
    {
        List<PlantTiles> potentialPlantPotsTiles = plantTileCollection.plantTiles.Where(p => p.isPlantPot).ToList();

        int rand = Random.Range(0, potentialPlantPotsTiles.Count);
        plantPot.plantTile = potentialPlantPotsTiles[rand];

        plantPotTileMap.SetTile(new Vector3Int(plantPot.x, plantPot.y, 0), plantPot.plantTile.tile);
    }
    
    public void UpdateTile(PlantPart part)
    {
        List<PlantTiles> potentialBranchTiles = null;
        List<PlantTiles> potentialFoliageTiles = null;

        potentialBranchTiles = plantTileCollection.plantTiles.Where(p =>
            (
                p.pointsUp == part.pointsUp
                && p.pointsRight == part.pointsRight
                && p.pointsDown == part.pointsDown
                && p.pointsLeft == part.pointsLeft
            )
            && !p.isPlantPot
            && !p.isFoliage
        ).ToList();

           
        if (potentialBranchTiles.Count > 0)
        {
            int rand = Random.Range(0, potentialBranchTiles.Count);
            part.plantTile = potentialBranchTiles[rand];
        }

        if ((part.foliageTile == null) && (part.foliage))
        {
            potentialFoliageTiles = plantTileCollection.plantTiles.Where(p =>
                p.isFoliage == part.foliage
            ).ToList();

            if (potentialFoliageTiles.Count > 0)
            {
                int rand = Random.Range(0, potentialFoliageTiles.Count);
                part.foliageTile = potentialFoliageTiles[rand];
            }
        }
    }

    public bool ShrinkPlant()
    {
        HashSet<PlantPart> visitedPlaces = new HashSet<PlantPart>();
        Queue<PlantPart> fringe = new Queue<PlantPart>();

        List<PlantPart> potentialShrinks = new List<PlantPart>();
        List<PlantPart> potentialFoliage = new List<PlantPart>();

        PlantPart current = null;
        fringe.Enqueue(plantPot);

        while (fringe.Count > 0)
        {
            current = fringe.Dequeue();

            if (
                (!current.plantPot) && (!current.foliage) &&
                (
                    ((current.pointsUp) && (!current.pointsRight) && (!current.pointsDown) && (!current.pointsLeft)) ||
                    ((!current.pointsUp) && (current.pointsRight) && (!current.pointsDown) && (!current.pointsLeft)) ||
                    ((!current.pointsUp) && (!current.pointsRight) && (current.pointsDown) && (!current.pointsLeft)) ||
                    ((!current.pointsUp) && (!current.pointsRight) && (!current.pointsDown) && (current.pointsLeft))
                )
               )
            {
                potentialShrinks.Add(plantGrid[current.x, current.y]);
            }
            else
            {
                if ((current.pointsUp) && ((current.plantPot) || !((current.x == current.parent.x) && (current.y + 1 == current.parent.y))))
                {
                    if ((current.y >= 0) && (current.y < plantGridSizeY) && (plantGrid[current.x, current.y + 1] != null))
                        fringe.Enqueue(plantGrid[current.x, current.y + 1]);
                }

                if ((current.pointsRight) && ((current.plantPot) || !((current.x + 1 == current.parent.x) && (current.y == current.parent.y))))
                {
                    if ((current.x >= 0) && (current.x < plantGridSizeX) && (plantGrid[current.x + 1, current.y] != null))
                        fringe.Enqueue(plantGrid[current.x + 1, current.y]);
                }

                if ((current.pointsDown) && ((current.plantPot) || !((current.x == current.parent.x) && (current.y - 1 == current.parent.y))))
                {
                    if ((current.y >= 0) && (current.y < plantGridSizeY) && (plantGrid[current.x, current.y - 1] != null))
                        fringe.Enqueue(plantGrid[current.x, current.y - 1]);
                }

                if ((current.pointsLeft) && ((current.plantPot) || !((current.x - 1 == current.parent.x) && (current.y == current.parent.y))))
                {
                    if ((current.x >= 0) && (current.x < plantGridSizeX) && (plantGrid[current.x - 1, current.y] != null))
                        fringe.Enqueue(plantGrid[current.x - 1, current.y]);
                }
            }

            // Foliage
            if (current.foliage)
            {
                potentialFoliage.Add(current);
            }

            visitedPlaces.Add(current);
        }

        if (potentialShrinks.Count == 0 && potentialFoliage.Count == 0)
        {
            return false;
        }

        PlantPart shrinkagePart = null;

        int rand = Random.Range(0, potentialShrinks.Count + (potentialFoliage.Count * 2));
        bool foliageWork = false;

        if (rand >= potentialShrinks.Count)
        {
            rand -= potentialShrinks.Count;
            rand >>= 1;

            shrinkagePart = potentialFoliage[rand];
            shrinkagePart.foliage = false;
            shrinkagePart.pointsFoliage = false;
            foliageWork = true;
        }
        else
        {
            shrinkagePart = potentialShrinks[rand];
        }

        if (!foliageWork)
        {
            if ((shrinkagePart.pointsDown) && (!shrinkagePart.parent.plantPot))
                shrinkagePart.parent.pointsUp = false;
            if (shrinkagePart.pointsLeft)
                shrinkagePart.parent.pointsRight = false;
            if (shrinkagePart.pointsUp)
                shrinkagePart.parent.pointsDown = false;
            if (shrinkagePart.pointsRight)
                shrinkagePart.parent.pointsLeft = false;

            // Drop a copy of the dying part that will fall off the screen
            var drop = Instantiate(DropPrefab, new Vector3(shrinkagePart.x - plantGridSizeX/2.0f + 1, shrinkagePart.y - plantGridSizeY/2.0f + 1, 0), Quaternion.identity);
            drop.GetComponent<SpriteRenderer>().sprite = shrinkagePart.plantTile.tile.sprite;

            UpdateTile(shrinkagePart);
            plantBranchTileMap.SetTile(new Vector3Int(shrinkagePart.x, shrinkagePart.y, 0), null);
            
            if (!shrinkagePart.parent.plantPot)
            {
                UpdateTile(shrinkagePart.parent);
                plantBranchTileMap.SetTile(new Vector3Int(shrinkagePart.parent.x, shrinkagePart.parent.y, 0), shrinkagePart.parent.plantTile.tile);
            }

            plantGrid[shrinkagePart.x, shrinkagePart.y] = null;
        }
        else
        {
            // Drop a copy of the dying part that will fall off the screen
            var drop = Instantiate(DropPrefab, new Vector3(shrinkagePart.x - plantGridSizeX / 2.0f + 1, shrinkagePart.y - plantGridSizeY / 2.0f + 1, 0), Quaternion.identity);
            drop.GetComponent<SpriteRenderer>().sprite = shrinkagePart.foliageTile.tile.sprite;

            shrinkagePart.foliageTile = null;
            plantFoliageTileMap.SetTile(new Vector3Int(shrinkagePart.x, shrinkagePart.y, 0), null);

            plantGrid[shrinkagePart.x, shrinkagePart.y] = shrinkagePart;
        }

        return true;
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
        return System.Math.Min(11 - day, 8);
    }

    public void UseTime()
    {
        freeTime -= 1;
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        GameObject.Find("DayIndicator").GetComponent<Text>().text = day.ToString();
        GameObject.Find("TimeIndicator").GetComponent<Text>().text = freeTime.ToString();
        GameObject.Find("GrowthIndicator").GetComponent<Text>().text = (100*growth/Settings.TotalGrowth).ToString() + "%";
    }

    public void StartDay()
    {
        day -= 1;
        freeTime = TimeOnDay(day);
        actionsToday.Clear();
        var splash = Instantiate(SplashPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        splash.GetComponent<Splash>().SetDay(day);
        InteractiveObject.popupOpen = false;
        pointsToday = 0;
        UpdateHUD();
    }

    public string hintAttention()
    {
        if (Attention > 0) return "Your plant craves attention, and will require it constantly. Remember - never turn your back on a plant!";
        else return "Your plant likes to be left alone. Don’t smother it! Plants, like teenagers, need some time to figure out who they are- and who they’re going to be.";
    }

    public string hintHunger()
    {
        if (Hunger > 0) return "Food, food, and more food! Your plant can eat anything. Just tuck it into the soil and let the plant do the rest!";
        else return "Do NOT try to give your plant food. It causes tremendous problems.";
    }

    public string hintThirst()
    {
        if (Thirst > 0) return "All plants need water, but yours needs more than most!";
        else return "Your plant’s natural habitat is the desert, and as such, it does its best work when there’s no water in sight.";
    }

    public string hintStimulation()
    {
        if (Stimulation > 0) return "This is a plant that needs an intense environment. Don't hold anything back.";
        else return "Your plant needs as little external stimulation as possible. Maybe get one of those sensory deprivation saltwater tanks?";
    }

    public string hintKnowledge()
    {
        if (Knowledge > 0) return "The more you know, the more you grow! Never has this been more true than with your new plant.";
        return "No secrets: your plant is not the brightest banana in the bunch, and we suspect it wants to be dumber.";
    }

    public string hintReassurance()
    {
        if (Reassurance > 0) return "Like anyone else, your plant just wants to know that it can love and be loved in return. Gestures of compassion are welcome.";
        return "Your plant likes to be jittery and freaked out. Try playing an ominous tune, or smiling at it in a weird way.";
    }

    public string hintPeace()
    {
        if (Peace > 0) return "A peaceful environment for your plant is paramount, making this a bad choice for any rock stars on tour, or extravagant house parties.";
        return "This plant likes it rowdy and loud. Consider starting a rock band, or a land war in Asia.";
    }

    public List<string> getHintsAt(int number)
    {
        var hints = new List<string>();
        if (Attention == number || Attention == -number) hints.Add(hintAttention());
        if (Hunger == number || Hunger == -number) hints.Add(hintHunger());
        if (Thirst == number || Thirst == -number) hints.Add(hintThirst());
        if (Stimulation == number || Stimulation == -number) hints.Add(hintStimulation());
        if (Knowledge == number || Knowledge == -number) hints.Add(hintKnowledge());
        if (Reassurance == number || Reassurance == -number) hints.Add(hintReassurance());
        if (Peace == number || Peace == -number) hints.Add(hintPeace());
        return hints;
    }

    public List<string> getHints(int target)
    {
        var hints = getHintsAt(3);

        if (hints.Count < target) {
            var fallback_hints = getHintsAt(2);
            while (fallback_hints.Count > 0 && hints.Count < target) {
                var index = Random.Range(0, fallback_hints.Count);
                hints.Add(fallback_hints[index]);
                fallback_hints.RemoveAt(index);
            }
        }

        if (hints.Count < target) {
            var fallback_hints = getHintsAt(1);
            while (fallback_hints.Count > 0 && hints.Count < target) {
                var index = Random.Range(0, fallback_hints.Count);
                hints.Add(fallback_hints[index]);
                fallback_hints.RemoveAt(index);
            }
        }

        if(hints.Count > target) {
            var index = Random.Range(0, hints.Count);
            hints.RemoveAt(index);
        }

        return hints;
    }

    public string GenerateStartingLetter()
    {
        var hints = getHints(Settings.Hints);

        int index = Random.Range(0, 1);
        switch(index) {
            default:
            case 0: {

                string hintString = "PS here is a clipping from the package to help you get started:\n";
                foreach(var hint in hints) {
                    hintString += "- " + hint + "\n";
                }

                string name = this.PlantName;
                if (!name.StartsWith("The")) {
                    if(name.StartsWith("A") || name.StartsWith("E") || name.StartsWith("I") || name.StartsWith("O") || name.StartsWith("Y")) name = "an " + name;
                    else name = "a " + name;
                }

                return string.Format(@"
Hello Dear!

I thought you might be lonely in the big city so I sent you a potted plant.
Its {0}! Thats my favorite kind. I'm going to visit in 10 days, I look 
forward to seeing how much it's grown in that time.

Love Mom <3

{1}", name, hintString);
            }
        } 
    }
}
