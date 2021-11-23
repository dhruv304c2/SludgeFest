using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Pun.UtilityScripts;

public class PlayerShooting : MonoBehaviourPun
{
    
    [Header("Requierd Objects")]
    
    [SerializeField] GameObject GunObject;
    [SerializeField] public Transform VaccumeForcePoint;
    [SerializeField] GameObject Projectile;
    [SerializeField] GameObject Grenade;
    [SerializeField] public Transform Gun;
    [SerializeField] Transform ThrowPosition;
    [SerializeField] GameObject MuzzelFlashObject;
    
    
    [Header("Wepon Attributes")]
    
    [SerializeField] float RecoilVelocity = 3f;
    [SerializeField] float MaxRecoilVelocity = 20;
    [SerializeField] float DepleationRate = 0.2f;
    [SerializeField] float RecoveryRate = 0.1f;
    [SerializeField] float RecoveryDelay = 0f;
    [SerializeField] float SwimmingRecoveryModifier = 2f;
    [SerializeField] public float Attractor = 10f;
    [SerializeField] float projectileVelocity = 20f;
    [SerializeField] float ThrowingVelocity = 50f;
    [SerializeField] float TBS = 0.1f;
    public int ItemCount;
    Animator anim;
    Rigidbody2D Rb;
    public float PaintinTank = 1f,RecoveryDelayTimer= 0f;
    public bool AllowRecovery = true;
    float ShotTimer = 10f;
    // Start is called before the first frame update
    
    void Start()
    {
        anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();
        Cursor.visible = false;
        if(!photonView.IsMine)
        {
            if (photonView.Owner.GetPlayerNumber() != PhotonNetwork.LocalPlayer.GetPlayerNumber())
            { gameObject.layer = LayerMask.NameToLayer("EnemyPlayer"); }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine )
        {
            ShotTimer += Time.deltaTime;
            anim.SetBool("Shoot", Input.GetButton("Fire1")&& PaintinTank > 0);
            if (Input.GetButton("Fire1") && PaintinTank > 0 && !anim.GetBool("Swimming"))
            {
                if (ShotTimer >= TBS)
                {
                    Vector2 AimDirection = GunObject.GetComponent<Aiming>().ReturnAim();
                    Vector2 absGunPosition = Gun.position;
                    photonView.RPC("Shoot", RpcTarget.All,AimDirection,absGunPosition);
                    ShotTimer = 0;
                    SetAllowRecovery(false);
                }
                    
            }
            if (Input.GetButtonDown("Fire3") && ItemCount > 0 && !anim.GetBool("Swimming"))
            {
                Vector2 AimDirection = GunObject.GetComponent<Aiming>().ReturnAim();
                Vector2 absGunPosition = ThrowPosition.position;
                photonView.RPC("ThrowGrenade", RpcTarget.All, AimDirection, absGunPosition);
            }

            RecoverySystem();
        }       
    }
    public float GetPaintInTank()
    {
        return PaintinTank;
    }
    public void SetAllowRecovery(bool Changeto)
    {
        AllowRecovery = Changeto;
    }

    [PunRPC] public void Shoot(Vector2 AimDirection, Vector2 absGunPosition)
    {
        var projectile = Instantiate(Projectile, absGunPosition, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = AimDirection * projectileVelocity;
        MuzzelFlash();
        CameraShake.cameraShake(0.6f, 0.1f,0.05f);
        if(!photonView.IsMine)
        {
            if(photonView.Owner.GetPlayerNumber() != PhotonNetwork.LocalPlayer.GetPlayerNumber())
            { projectile.layer = LayerMask.NameToLayer("EnemyProjectile"); }
        }
    }

    [PunRPC]
    public void ThrowGrenade(Vector2 AimDirection, Vector2 absGunPosition)
    {
        var projectile = Instantiate(Grenade, absGunPosition, Quaternion.identity);
        ItemCount--;
        projectile.GetComponent<Rigidbody2D>().velocity = AimDirection * ThrowingVelocity;
        if (!photonView.IsMine)
        {
            if (photonView.Owner.GetPlayerNumber() != PhotonNetwork.LocalPlayer.GetPlayerNumber())
            { projectile.layer = LayerMask.NameToLayer("EnemyProjectile"); }
        }
    }
    
    public void RecoverySystem()
    {
        if (!AllowRecovery)
        {
            RecoveryDelayTimer += Time.deltaTime;
            if (RecoveryDelayTimer >= RecoveryDelay)
            {
                AllowRecovery = true;
                RecoveryDelayTimer = 0f;
            }
        }
        if (anim.GetBool("Shoot"))
        {
            PaintinTank = Mathf.MoveTowards(PaintinTank, 0f, DepleationRate * Time.deltaTime);
        }
        else if (AllowRecovery)
        {
            float ModifiedRecoveryRate = 0f;
            if (anim.GetBool("Swimming"))
            {
                ModifiedRecoveryRate = RecoveryRate * SwimmingRecoveryModifier;
            }
            else
            {
                ModifiedRecoveryRate = RecoveryRate;
            }
            PaintinTank = Mathf.MoveTowards(PaintinTank, 1f, ModifiedRecoveryRate * Time.deltaTime);
        }
    }
    
    public int GetItemCount()
    {
        return ItemCount;
    }
    
    public void AddItem(int Amount)
    {
        ItemCount += Amount;
    }

    private void MuzzelFlash()
    {
        Instantiate(MuzzelFlashObject, Gun.position, GunObject.transform.rotation);
    }

}
