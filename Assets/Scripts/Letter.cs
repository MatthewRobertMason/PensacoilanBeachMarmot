using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter : MonoBehaviour
{
    public Sprite openLetter;

    // Probably static
    public GameObject popupPrefab;
    public Texture2D cursor;
    public string letterText = "";

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
        if (!InteractiveObject.popupOpen) {
            GetComponent<SpriteRenderer>().sprite = openLetter;
            GameObject popupObj = Instantiate(popupPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            InteractiveObject.popupOpen = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            Popup popup = popupObj.GetComponent<Popup>();
            popup.SetHeader("");
            popup.SetExitText("Okay");
            popup.SetBody(letterText);
        }
    }

    void OnMouseOver()
    {
        if (!InteractiveObject.popupOpen) Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        if (!InteractiveObject.popupOpen) Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
