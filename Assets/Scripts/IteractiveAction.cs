using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IteractiveAction : MonoBehaviour
{
    ObjectInteraction interaction;
    public Texture2D cursor;
    public Color hoverColour;
    public bool active;

    private Color defaultColour;

    // Start is called before the first frame update
    void Start()
    {
        defaultColour = GetComponent<Text>().color;
    }

    public void SetAction(ObjectInteraction oi)
    {
        interaction = oi;
        defaultColour = GetComponent<Text>().color;
        GetComponent<Text>().text = oi.InteractionDescription;
        active = !YourPlant.GetInstancePlant().actionsToday.Contains(oi.InteractionDescription);
        if (!active) {
            defaultColour.a = 0.1f;
            GetComponent<Text>().color = defaultColour;
        }
    }

    public void MouseOver()
    {
        if (active) {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
            GetComponent<Text>().color = hoverColour;
        }
    }

    public void MouseExit()
    {
        if (active) {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            GetComponent<Text>().color = defaultColour;
        }
    }

    public void Press() {
        if (active) {
            YourPlant.GetInstancePlant().ApplyInteration(interaction);
            this.GetComponentInParent<Popup>().Remove();
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}
