using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    [SerializeField] float DestroyTime = 2f;
    [SerializeField] bool DestoryOnSignal=false;
    
    // Start is called before the first frame update
    void Start()
    {
        if(!DestoryOnSignal)
            Invoke("DestroyThis", DestroyTime);           
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}
