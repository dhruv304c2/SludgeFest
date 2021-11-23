using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkAiming : MonoBehaviourPun, IPunObservable
{
    public Quaternion RemoteRotation;
    public float LagRotation;
    public float LagPosition;
    private void Awake()
    {
        if (!photonView.IsMine || photonView.Owner != PhotonNetwork.LocalPlayer)
            Destroy(GetComponent<Aiming>());
    }

    public void Update()
    {
        if (!photonView.IsMine)
        {
            LagRotation = Quaternion.Angle(RemoteRotation, transform.rotation);

            if (LagRotation > 1f)
            {
                transform.rotation = RemoteRotation;
            }
            else if (LagRotation < 0.01f)
            {
               
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, RemoteRotation, Time.deltaTime);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {          
            stream.SendNext(transform.rotation);
        }
        else
        {      
            RemoteRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}

