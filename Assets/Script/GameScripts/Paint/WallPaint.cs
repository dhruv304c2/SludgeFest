using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPaint : MonoBehaviour
{
    [SerializeField] float maxScale;
    [SerializeField] float minScale;
    [SerializeField] bool RandomRotation = false;
    // Start is called before the first frame update
    void Start()
    {
        float scale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(scale, scale, 0);
        if(RandomRotation)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        }
    }

}
