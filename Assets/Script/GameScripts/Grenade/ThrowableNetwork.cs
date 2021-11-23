using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ThrowableNetwork : MonoBehaviourPun, IPunObservable
{
    private float ThrowingVelocity;

    public Vector3 RemoteObjectPosition;
    private float LagDistance;

    void Awake()
    {
        ThrowingVelocity = GetComponent<Grenade>().ThrowingVelocity;
        if (!photonView.IsMine)
        {
            Destroy(GetComponent<Grenade>());
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            LagDistance = Vector3.Distance(RemoteObjectPosition, transform.position);

            if (LagDistance > 5f)
            {
                transform.position = RemoteObjectPosition;
            }
            else if (LagDistance > 0.5f)
            {
                transform.position = Vector3.Lerp(transform.position, RemoteObjectPosition, Time.deltaTime * ThrowingVelocity);
            }
            else if (LagDistance > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, RemoteObjectPosition, Time.deltaTime * ThrowingVelocity / 2);
            }
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            RemoteObjectPosition = (Vector3)stream.ReceiveNext();
        }
    }
}
