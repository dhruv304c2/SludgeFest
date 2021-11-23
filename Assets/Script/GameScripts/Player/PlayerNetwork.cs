using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Security.Cryptography;

public class PlayerNetwork : MonoBehaviourPun, IPunObservable
{
    // Start is called before the first frame update
    public Quaternion RemoteRotation;
    public Vector3 Remoteposition;
    public float LagRotation;
    public float LagPosition;

    [SerializeField] GameObject LeftHat;
    [SerializeField] GameObject RightHat;
    [SerializeField] GameObject Hat;
    [SerializeField] GameObject GunGraphic; 

    bool LeftActive;
    bool RightActive;
    bool HatActive;
    float GunScaleY;

    float playerSpeed;
    float Multiplier;
    float currentSpeed;
    Animator anim;
    float PlayerHealth;
    float RemotePlayerHealth;

    private void Awake()
    {
        playerSpeed = GetComponent<PlayerMovement>().PlayerSpeed;
        Multiplier = GetComponent<PlayerMovement>().SwimSpeedModifier;        
        anim = GetComponent<Animator>();
        if(!photonView.IsMine||photonView.Owner!=PhotonNetwork.LocalPlayer)
        {
            Destroy(GetComponent<CameraControls>());
            Destroy(GetComponent<Interactions>());
            Destroy(GetComponent<PlayerMovement>());
            GetComponent<Rigidbody2D>().isKinematic = true;
        }
    }

    public void Update()
    {
        if (!photonView.IsMine)
        {
            transform.rotation = RemoteRotation;

            LagPosition = Vector3.Distance(transform.position, Remoteposition);
            
            if(LagPosition > 20f)
            {
                transform.position = Remoteposition;
            }
            else if (LagPosition > 15f)
            {
                transform.position = Vector3.Lerp(transform.position, Remoteposition, Time.deltaTime * playerSpeed * 4f);
            }
            else if(LagPosition > 10f)
            {
                transform.position = Vector3.Lerp(transform.position, Remoteposition, Time.deltaTime * playerSpeed * 3f);
            }
            else if(LagPosition > 5f)
            {
                transform.position = Vector3.Lerp(transform.position, Remoteposition, Time.deltaTime * playerSpeed * 2f);
            }
            else if(LagPosition > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, Remoteposition, Time.deltaTime * playerSpeed);
            }
            else
            {
            }

            LeftHat.SetActive(LeftActive);
            RightHat.SetActive(RightActive);
            Hat.SetActive(HatActive);
            GunGraphic.transform.localScale = new Vector3(transform.localScale.x,GunScaleY,1f); 
            GetComponent<PlayerHealth>().Health = RemotePlayerHealth;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(LeftHat.activeSelf);
            stream.SendNext(RightHat.activeSelf);
            stream.SendNext(PlayerHealth);
            stream.SendNext(Hat.activeSelf);
            stream.SendNext(GunGraphic.transform.localScale.y);
        }
        else
        {
            Remoteposition = (Vector3)stream.ReceiveNext();
            RemoteRotation = (Quaternion)stream.ReceiveNext();
            LeftActive = (bool)stream.ReceiveNext();
            RightActive = (bool)stream.ReceiveNext();
            RemotePlayerHealth = (float)stream.ReceiveNext();
            HatActive = (bool)stream.ReceiveNext();
            GunScaleY = (float)stream.ReceiveNext();
        }
    }
}
