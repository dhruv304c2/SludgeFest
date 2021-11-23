using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviourPun
{
    public float Health { get; set; } = 1f ;

    [Header("Player Graphics...")]
    [SerializeField] List<SpriteRenderer> PlayerGraphics;
    [SerializeField] List<ParticleSystem> DeathParticles;
    [SerializeField] GameObject nameTag;
    [SerializeField] Animator anim;

    [Header("Flash Material...")]
    [SerializeField] Material Flashmaterial;
    [SerializeField] float flashDuration;

    [Header("Player Respawn...")]
    [SerializeField] float RespawnTime = 5f;
    // Player Respawn Point.....
    public Vector2 Respawnpoint;
    Color defColor;
    Material defMaterial;
    bool Dead = false;

    void Start()
    {
        defMaterial = PlayerGraphics[0].material;
        Respawnpoint = transform.position;
    }

    // Update is called once per frame

    public void DealDamageOnNetwork(float Damage)
    {
        photonView.RPC("DealDamage", RpcTarget.All, Damage);
    }
    
    [PunRPC] public void DealDamage(float Damage)
    {
        if (photonView.IsMine)
        {
            if (!anim.GetBool("Swimming"))
            {
                Health -= Damage;
                if (Health <= 0 && !Dead)
                {
                    photonView.RPC("Die", RpcTarget.All);
                }
            }
        }
        Flash();
    }

    public void Flash()
    {
        foreach(SpriteRenderer Graphic in PlayerGraphics)
        {
            Graphic.material = Flashmaterial;
        }
        Invoke("ResetMaterial", flashDuration);
    }

    public void ResetMaterial()
    {
        foreach (SpriteRenderer Graphic in PlayerGraphics)
        {
            Graphic.material = defMaterial;
        }
    }

    [PunRPC] public void Die()
    {
        Dead = true;
        foreach (SpriteRenderer Graphic in PlayerGraphics)
        {
            Graphic.enabled = false;
        }
        anim.enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        nameTag.SetActive(false);
        
        foreach(ParticleSystem particleSystem in DeathParticles)
        {
            particleSystem.Play();
        }
        photonView.RPC("RespawnOnNetwork", RpcTarget.All);
    }

    [PunRPC] public void RespawnOnNetwork()
    {
        Invoke("Respawn", RespawnTime);
    }
    
    public void Respawn()
    {
        Dead = false;
        foreach (SpriteRenderer Graphic in PlayerGraphics)
        {
            Graphic.enabled = true;
        }
        anim.enabled = true;
        nameTag.SetActive(true);
        transform.position = Respawnpoint;
        GetComponent<Rigidbody2D>().isKinematic = false;
        Health = 1f;
        
    }
    public float GetHealth()
    {
        return Health;
    }
}
