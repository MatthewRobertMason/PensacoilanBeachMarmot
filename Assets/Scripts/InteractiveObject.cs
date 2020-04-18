using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ObjectInteraction
{
    public string InteractionDescription;
    public int Attention;
    public int Hunger;
    public int Thirst;
    public int Stimulation;
    public int Knowledge;
    public int Reassurance;
    public int Peace;
}


public class InteractiveObject : MonoBehaviour
{
    public string InteractionHeader;
    public ObjectInteraction[] Interations;

    public GameObject PopupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        GameObject popup = Instantiate(PopupPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        popup.GetComponent<Popup>().SetHeader(InteractionHeader);
    }
}
