using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class ParticleColisionHandeller : MonoBehaviourPun
{
    [SerializeField] Transform SplatHolder;
    [SerializeField] GameObject SplatPrefab;
    [SerializeField] GameObject Particles;
    [SerializeField] string Color;
    [SerializeField] float DamagePP = 0.01f;
    [SerializeField] LayerMask SurfaceCheckMask;
    [SerializeField] WallPaint wallPaint;
    [SerializeField] Color WallPaintColor;
    [SerializeField] List<GameObject> FXLists;
    private void Start()
    {
        SplatHolder = FindObjectOfType<SplatHolder>().transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.IsMine)
        {
            GameObject other = collision.gameObject;

            if (other.GetComponent<CapturePoint>())
            {
                other.GetComponent<CapturePoint>().AttackedByTeam(Color);
            }

            else if (other.GetComponent<FloorPaintChanger>())
            {
                if (other.GetComponent<FloorPaintChanger>().GetCurrentColor() != Color)
                {
                    other.GetComponent<FloorPaintChanger>().ChangePaintColor(Color);
                }
                
            }
            else if (photonView.Owner != PhotonNetwork.LocalPlayer)
            { 
                if (other.GetComponent<PlayerHealth>())
                {
                    other.GetComponent<PlayerHealth>().DealDamageOnNetwork(DamagePP);                    
                }
            }
        }
        InstantiateSplat(transform.position);
        FormSplatter(transform.position);
        ShowFX();
        Particles.transform.parent = null;
        Particles.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }

    public void InstantiateSplat(Vector2 abspos)
    {
        if (SplatPrefab)
        {
            var Splat = Instantiate(SplatPrefab, abspos, Quaternion.identity) as GameObject;
            Splat.transform.SetParent(SplatHolder, true);
        }
    }
    public void FormSplatter(Vector2 Position)
    {
        GameObject Paint = Instantiate(wallPaint.gameObject, transform.position, Quaternion.Euler(0f,0f,Random.Range(0f,360f)));
        Paint.GetComponent<SpriteRenderer>().color = WallPaintColor;
    }

    public void ShowFX()
    {
        foreach(GameObject Fx in FXLists)
        {
            Instantiate(Fx, transform.position, Quaternion.identity);
        }
    }
}
