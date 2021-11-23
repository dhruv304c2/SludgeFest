using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontScaleWithParent : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(transform)
        transform.localScale = 
        new Vector3(1 / Mathf.Clamp(transform.parent.localScale.x,0.001f,1f), 1 / 1 / Mathf.Clamp(transform.parent.localScale.y, 0.001f, 1f));
    }
}
