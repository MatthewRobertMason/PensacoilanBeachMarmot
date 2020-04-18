using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourPlant : MonoBehaviour
{
    public string PlantName;

    private static YourPlant the_plant;
    public static YourPlant GetInstancePlant()
    {
        return the_plant;
    }


    // Start is called before the first frame update
    void Start()
    {
        the_plant = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyInteration(ObjectInteraction interaction)
    {
        Debug.Log("Did Thing");
    }
}
