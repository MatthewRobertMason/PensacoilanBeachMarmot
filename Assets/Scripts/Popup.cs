using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject Header;

    public void Remove()
    {
        Object.Destroy(this.gameObject);
        InteractiveObject.popupOpen = false;
    }

    public void SetHeader(string text)
    {
        Header.GetComponent<Text>().text = text;
    }
}
