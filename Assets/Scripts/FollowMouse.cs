using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = new Vector3(position.x, position.y, 0);
    }
}
