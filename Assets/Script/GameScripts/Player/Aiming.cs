using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviourPun
{
    // Start is called before the first frame update
    [SerializeField] Camera PlayerCamera;
    [SerializeField] Transform GunPoint;
    float startYpos;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            ReturnAim();
        }
    }
        
    public Vector2 ReturnAim()
    {
        Vector2 MousePos = Input.mousePosition;
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 WorldPoint = new Vector2(PlayerCamera.ScreenToWorldPoint(MousePos).x, PlayerCamera.ScreenToWorldPoint(MousePos).y);
        Vector2 RelativeVector =  (WorldPoint- pos).normalized;
        float Angle = Mathf.Atan2(RelativeVector.y, RelativeVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, Angle);        
        return RelativeVector;
    }
}
