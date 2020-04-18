using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTileCollection : MonoBehaviour
{
    public PlantTiles[] plantTiles;

    // Start is called before the first frame update
    void Start()
    {
        foreach (PlantTiles p in plantTiles)
        {
            if (p.isPlantPot)
            {
                p.isFoliage = false;

                p.pointsUp = true;
                p.pointsRight = false;
                p.pointsDown = false;
                p.pointsLeft = false;
            }

            if (p.isFoliage)
            {
                p.pointsUp = false;
                p.pointsRight = false;
                p.pointsDown = false;
                p.pointsLeft = false;
            }
        }
    }
}
