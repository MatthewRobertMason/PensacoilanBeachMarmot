using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IteractiveAction : MonoBehaviour
{
    ObjectInteraction interaction;
    public Texture2D cursor;
    public Color hoverColour;

    private Color defaultColour;

    // Start is called before the first frame update
    void Start()
    {
        defaultColour = GetComponent<Text>().color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAction(ObjectInteraction oi)
    {
        interaction = oi;
        GetComponent<Text>().text = oi.InteractionDescription;
    }

    public void MouseOver()
    {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        GetComponent<Text>().color = hoverColour;
    }

    public void MouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        GetComponent<Text>().color = defaultColour;
    }

    public void Press() {
        YourPlant.GetInstancePlant().ApplyInteration(interaction);
        this.GetComponentInParent<Popup>().Remove();
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
