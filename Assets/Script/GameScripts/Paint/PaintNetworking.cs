using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PaintNetworking : MonoBehaviourPun,IPunObservable
{
    public Quaternion RemoteRotation;
    public Vector3 Remoteposition;
    public float LagRotation;
    public float LagPosition;
    public GameObject parent;
    private void Awake()
    {

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
                return;
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, RemoteRotation, 0.1f);
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
