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
    public string interactionHeader;
    public ObjectInteraction[] interations;
    
    // Probably static
    public GameObject popupPrefab;
    public Texture2D cursor;
    public static bool popupOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        popupOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUpAsButton()
    {
        if (!popupOpen) {
            GameObject popupObj = Instantiate(popupPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            popupOpen = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            Popup popup = popupObj.GetComponent<Popup>();
            popup.SetHeader(interactionHeader);

            foreach (ObjectInteraction oi in interations) {
                popup.AddInteraction(oi);
            }
            popup.AddInteractionPadding();
        }
    }

    void OnMouseOver()
    {
        if(!popupOpen) Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        if (!popupOpen) Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
