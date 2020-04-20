using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPart : MonoBehaviour
{

    private float createdAt;

    // Start is called before the first frame update
    void Start()
    {
        createdAt = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - createdAt > 10) {
            GameObject.Destroy(gameObject);
        }
    }
}
