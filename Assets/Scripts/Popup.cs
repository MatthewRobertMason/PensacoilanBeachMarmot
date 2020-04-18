using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    public GameObject Header;
    public GameObject body;
    public GameObject optionPrefab;

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
}
