using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject Header;
    public GameObject body;
    public GameObject fullBody;
    public GameObject fullBodyContainer;
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
        fullBodyContainer.SetActive(true);
        //var textObject = new GameObject();
        var text = this.fullBody.AddComponent<Text>();
        var size_fitter = this.fullBody.AddComponent<ContentSizeFitter>();
        size_fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        //textObject.AddComponent<Scrollbar>();
        text.text = body.Trim();
        text.font = font;
        text.color = color;
        text.fontSize = 20;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        //textObject.transform.SetParent(this.fullBody.transform);
    }
}
