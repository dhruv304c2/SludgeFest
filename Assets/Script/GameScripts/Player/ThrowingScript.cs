using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ThrowingScript : MonoBehaviourPun
{
    [SerializeField] float ThrowVelocity = 20f;
    [SerializeField] GameObject Throwable;
    [SerializeField] Transform ThrowPoint;
    [SerializeField] Camera PlayerCamera;
    [SerializeField] Rigidbody2D Player;
    [SerializeField] GameObject Gun;

    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            if(Input.GetButtonDown("Fire3"))
            {
                photonView.RPC("ThrowObject", RpcTarget.All);                
            }
        }
    }

    [PunRPC] public void ThrowObject()
    {
        var ObjectThrown = Instantiate(Throwable, ThrowPoint.position, Quaternion.identity);
        Vector2 AimDirection = Gun.GetComponent<Aiming>().ReturnAim();
        ObjectThrown.GetComponent<Rigidbody2D>().velocity = AimDirection.normalized * ThrowVelocity;
        ObjectThrown.GetComponent<Grenade>().ThrowingVelocity = ThrowVelocity;
    }
}
