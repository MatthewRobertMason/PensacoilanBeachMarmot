using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Splash : MonoBehaviour
{
    public GameObject DayCounter;
    private float start;

    // Start is called before the first frame update
    void Start()
    {
        start = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - start > 3) {
            Remove();
        }
    }

    public void SetDay(int day)
    {
        DayCounter.GetComponent<Text>().text = day.ToString();
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
