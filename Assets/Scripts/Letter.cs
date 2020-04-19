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

    private bool new_letter = true;
    private float bump_time = 0;
    private float target_rotation = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (new_letter) {
            if(bump_time < Time.time) {
                target_rotation = 35;
                bump_time = Time.time + 4;
            }

            float current = Mathf.LerpAngle(this.transform.rotation.eulerAngles.z, target_rotation, 0.12f);
            this.transform.rotation = Quaternion.Euler(0, 0, current);

            if(Mathf.Abs(Mathf.DeltaAngle(current, target_rotation)) < 0.5) {
                target_rotation = target_rotation * -0.8f;
            }

        } else {
            var rot = this.transform.rotation;
            rot.z = 0;
            this.transform.rotation = rot;
        }
    }

    private void OnMouseUpAsButton()
    {
        if (!InteractiveObject.popupOpen) {
            GetComponent<SpriteRenderer>().sprite = openLetter;
            new_letter = false;

            GameObject popupObj = Instantiate(popupPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            InteractiveObject.popupOpen = true;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

            Popup popup = popupObj.GetComponent<Popup>();
            popup.SetHeader("");
            popup.SetExitText("Thanks");
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
