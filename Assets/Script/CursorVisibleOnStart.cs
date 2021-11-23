using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorVisibleOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;        
    }

    private void Update()
    {
        Cursor.visible = true;
    }
}
