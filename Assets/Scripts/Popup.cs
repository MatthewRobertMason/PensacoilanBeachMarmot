using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject Header;
    public GameObject body;
    public GameObject optionPrefab;
    public GameObject exitText;
    public Color color;
    public Font font;

    public void Remove()
    {
        Object.Destroy(this.gameObject);
        InteractiveObject.popupOpen = false;
    }

    public void SetHeader(string text)
    {
        Header.GetComponent<Text>().text = text;
    }

    public void AddInteraction(ObjectInteraction oi)
    {
        GameObject option = Instantiate(optionPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        option.GetComponent<IteractiveAction>().SetAction(oi);
        option.transform.SetParent(body.transform);
    }

    public void AddInteractionPadding()
    {
        GameObject obj = new GameObject();
        obj.transform.SetParent(body.transform);
    }

    public void SetExitText(string text)
    {
        exitText.GetComponent<Text>().text = text;
    }

    public void SetBody(string body)
    {
        var textObject = new GameObject();
        var text = textObject.AddComponent<Text>();
        text.text = body;
        text.font = font;
        text.color = color;
        text.fontSize = 20;
        textObject.transform.SetParent(this.body.transform);
    }
}
