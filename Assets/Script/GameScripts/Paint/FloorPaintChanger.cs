using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FloorPaintChanger : MonoBehaviourPun
{
    [SerializeField] List<GameObject> PaintObjects;
    [SerializeField] string currentColor;
    
    // Start is called before the first frame update
    public void ChangePaintColor(string color)
    {
        if(currentColor != color)
            photonView.RPC("ChangeColorOnNetwork", RpcTarget.All,color);        
    }

    public void Activate(GameObject paint)
    {
        paint.GetComponent<Animator>().SetBool("Active", true);
    }
    
    [PunRPC] private void DeactiveatePaint()
    {
        
        foreach (GameObject Paint in PaintObjects)
        {
            gameObject.layer = LayerMask.NameToLayer(currentColor);
            if (Paint.name != currentColor)
            {
                Paint.GetComponent<Animator>().SetBool("Active", false);
                Paint.GetComponent<SpriteRenderer>().sortingOrder = 0;
            }
        }
    }

    public string GetCurrentColor()
    {
        return currentColor;
    }

    [PunRPC] public void ChangeColorOnNetwork(string Color)
    {
        foreach (GameObject Paint in PaintObjects)
        {
            currentColor = Color;
            if (Paint.name == Color)
            {
                Activate(Paint);
                Paint.GetComponent<SpriteRenderer>().sortingOrder = 1;
                gameObject.layer = LayerMask.NameToLayer(Color);
                DeactiveatePaint();
            }
        }
    }
}
