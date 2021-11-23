using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstantScaleTexture : MonoBehaviour
{
    Material MyMaterial;
    // Start is called before the first frame update
    void Start()
    {
        MyMaterial = GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        MyMaterial.SetFloat("_YTile", transform.localScale.y);
        MyMaterial.SetFloat("_XTile", transform.localScale.x);
    }
}
