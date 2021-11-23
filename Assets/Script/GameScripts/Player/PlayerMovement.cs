using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;
using Photon.Pun;
using JetBrains.Annotations;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] public float PlayerSpeed = 5f, JumpBoost = 5f, InAirModifier = 0.8f, DissolveStableTime = 0.1f,SwimSpeedModifier=2f,minDistanceToSwim=1f,RoationSpeed = 5f;
    float CurrentPlayerSpeed,RecoveryDelayTimer=0f;
    Animator anim;
    Collider2D feet,body;
    public bool IsTouchingGround = true, CanDissolve = false, Swimming=false,IsTouchingWall = false,WallRunning=false,AllowRecovery=true,IsOnTeamsGround=false;
    public bool CanSwim;
    int JumpCounter = 0;
    Rigidbody2D MyRb;
    public float Energy= 1f;

    // Layer Mask for ground layers 
    int GroundLayerMask = 1 << 12 |1 << 13 | 1 << 14 | 1 << 16 ; 

    [Header("Player Info...")]
    
    [SerializeField] public string Type = "";
    
    [Header("Requierd Fields...")]
    
    [SerializeField] int MaxJumps = 2;
    [SerializeField] List<SpriteRenderer> GunGraphics;
    [SerializeField] GameObject Hat;
    [SerializeField] GameObject WallHatRight,WallHatLeft;
    [SerializeField] float WallCheckDistance = 3f;
    [SerializeField] float EnergyDepleationRate = 0.2f ,EnergyRecoveryRate= 0.1f, RecoveryDelay=0.3f;
    [SerializeField] Transform RaycastPosition;
    [SerializeField] public Camera PlayerCam;
        
    [Header("Particles....")]
    
    [SerializeField] ParticleSystem SwimParticles;
    [SerializeField] ParticleSystem SwimOutPrticles;
    [SerializeField] ParticleSystem JumpParticle;
    [SerializeField] ParticleSystem RunParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        feet = GetComponent<BoxCollider2D>();
        body = GetComponent<PolygonCollider2D>();
        CurrentPlayerSpeed = PlayerSpeed;
        MyRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
            
        if (photonView.IsMine)
        {
            HandleBools();
            if (!AllowRecovery && !Swimming)
            {
                RecoveryDelayTimer += Time.deltaTime;
                if (RecoveryDelayTimer >= RecoveryDelay)
                {
                    AllowRecovery = true;
                    RecoveryDelayTimer = 0f;
                }
            }
            if (Swimming)
            {
                Energy = Mathf.MoveTowards(Energy, 0f, EnergyDepleationRate * Time.deltaTime);
            }
            else if (AllowRecovery)
            {
                Energy = Mathf.MoveTowards(Energy, 1f, EnergyRecoveryRate * Time.deltaTime);
            }
           
            if (!Swimming)
            {
                WallRunning = false;
            }

                
            Swim();
            Movement();
            if (Input.GetButtonDown("Jump"))
            {
                if (JumpCounter < MaxJumps)
                {
                    if (!Swimming)
                        anim.SetBool("Jump", true);
                }
            }
            if (!WallRunning)
                LookAtMouse();
            IsWallRight();
            IsWallLeft();
        }
        
    }


    [PunRPC]
    
    public void Jump()
    {
        anim.SetBool("Jump", false);        
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, JumpBoost);
        JumpCounter++;
    }

    [PunRPC]

    private void LookAtMouse()
    {
        if (!Swimming)
        {
            Vector2 MousePos;
            MousePos = PlayerCam.ScreenToWorldPoint(Input.mousePosition);
            if (transform.position.x <= MousePos.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                FlipGunGraphicRight();
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                FlipGunGraphicLeft();
            }
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    
    [PunRPC]
    
    private void Movement()
    {
        if (IsTouchingGround)
        {
            CurrentPlayerSpeed = PlayerSpeed;
            JumpCounter = 0;
        }
        else
        {
            CurrentPlayerSpeed = PlayerSpeed * InAirModifier;
        }
        if(Swimming)
        {
            CurrentPlayerSpeed = PlayerSpeed * SwimSpeedModifier;
        }
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SwimOut"))
        {
            if (Input.GetButton("Horizontal"))
            {
                if (IsTouchingGround)
                {
                    if (!Swimming)
                    {
                        anim.SetBool("Walk", true);
                    }
                }
                if (Swimming)
                {   
                    if( (IsTouchingWall || WallRunning))
                    {
                        float Speed2 = CurrentPlayerSpeed * Input.GetAxis("Horizontal");
                        if (WallRunning)
                        {
                            if (IsTouchingGround)
                            {
                                if (IsWallRight() && Speed2 < 0f)
                                {
                                    float Speed1 = CurrentPlayerSpeed * Input.GetAxis("Horizontal");
                                    transform.position = transform.position + new Vector3(Time.deltaTime * Speed1, 0f, 0f);
                                    WallRunning = false;
                                }
                                if (IsWallLeft() && Speed2 > 0f)
                                {
                                    float Speed1 = CurrentPlayerSpeed * Input.GetAxis("Horizontal");
                                    transform.position = transform.position + new Vector3(Time.deltaTime * Speed1, 0f, 0f);
                                    WallRunning = false;
                                }
                            }
                            else
                            {
                                if (IsWallRight())
                                {
                                    transform.position = transform.position + new Vector3(0f, Time.deltaTime * Speed2, 0f);
                                    WallRunning = true;
                                }
                                else if (IsWallLeft())
                                {
                                    transform.position = transform.position + new Vector3(0f, -Time.deltaTime * Speed2, 0f);
                                    WallRunning = true;
                                }
                                else
                                    WallRunning = false;
                            }
                        }
                        else
                        {
                            if (IsWallRight())
                            {
                                transform.position = transform.position + new Vector3(0f, Time.deltaTime * Speed2, 0f);
                                WallRunning = true;
                            }
                            else if (IsWallLeft())
                            {
                                transform.position = transform.position + new Vector3(0f, -Time.deltaTime * Speed2, 0f);
                                WallRunning = true;
                            }
                        }
                    }
                    else
                    {
                        float Speed1 = CurrentPlayerSpeed * Input.GetAxis("Horizontal");
                        transform.position = transform.position + new Vector3(Time.deltaTime * Speed1, 0f, 0f);
                         WallRunning = false;
                    }
                }
                else
                {
                    float Speed = CurrentPlayerSpeed * Input.GetAxis("Horizontal");
                    transform.position = transform.position + new Vector3(Time.deltaTime * Speed, 0f, 0f);
                }
            }
            else
                anim.SetBool("Walk", false);
        }
    }
    [PunRPC] private void FlipGunGraphicLeft()
    {
        foreach (SpriteRenderer GunGraphic in GunGraphics)
        {
            GunGraphic.transform.localScale = new Vector3(1f, -1f, 1f);
        }
    }
    [PunRPC] private void FlipGunGraphicRight()
    {
        foreach (SpriteRenderer GunGraphic in GunGraphics)
        {
            GunGraphic.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    [PunRPC]

    private void Swim()
    {
        if (CanDissolve && (IsTouchingGround||IsTouchingWall)&& Energy!=0 && Input.GetButton("Vertical"))
        {
            Swimming = true;
            AllowRecovery = false;            
        }
        else 
        {
            MyRb.velocity = new Vector2(0, MyRb.velocity.y);
            Swimming = false;
        }

    }
    public void PlaySwimParticles()
    {
        SwimParticles.Play();
    }
    public void PlaySwimOutParticles()
    {
        SwimOutPrticles.Play();
    }
    public void PlayJumpParticles()
    {
        JumpParticle.Play();
    }
    public void RotateToangle(float rotationSpeed,float targetAngle)
    {
        if (transform.rotation.eulerAngles.z != targetAngle)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x,
                                                 transform.rotation.eulerAngles.y,
                                                 Mathf.Lerp(transform.rotation.eulerAngles.z,
                                                 targetAngle, Time.deltaTime * rotationSpeed));
        }
    }
    public bool IsWallRight()
    {
        RaycastHit2D Rightwall = Physics2D.Raycast(RaycastPosition.position, Vector2.right, WallCheckDistance,LayerMask.GetMask(Type,"Wall")|GroundLayerMask);
        if (Rightwall.collider != null)
        {
            if (Rightwall.collider.gameObject.CompareTag("Walls"))
            {
                Debug.DrawRay(RaycastPosition.position, Vector2.right * Rightwall.distance, Color.red);

                //Custom Wall Collision

                if (!Swimming)
                    transform.position = new Vector2(transform.position.x - Time.deltaTime * CurrentPlayerSpeed, transform.position.y);
                
                if (Rightwall.collider.gameObject.layer == LayerMask.NameToLayer(Type))
                {
                    CanDissolve = true;
                }
                else
                {
                    CanDissolve = false;
                }
                return true;
            }
            else
            {
                Debug.DrawRay(RaycastPosition.position, Vector2.right * WallCheckDistance, Color.red);
                return false;
            }
        }
        else
        {
            Debug.DrawRay(RaycastPosition.position, Vector2.right * WallCheckDistance, Color.red);
            return false;
        }
        
    }
    public bool IsWallLeft()
    {
        
        RaycastHit2D LeftWall = Physics2D.Raycast(RaycastPosition.position, Vector2.left, WallCheckDistance, LayerMask.GetMask(Type,"Wall") | GroundLayerMask);
        if (LeftWall.collider != null)
        {
            if (LeftWall.collider.gameObject.CompareTag("Walls"))
            {
                Debug.DrawRay(RaycastPosition.position, Vector2.left * LeftWall.distance, Color.blue);
                
                //Custom Wall Collision
                
                if (!Swimming)
                    transform.position = new Vector2(transform.position.x+Time.deltaTime*CurrentPlayerSpeed, transform.position.y);
                
                if (LeftWall.collider.gameObject.layer == LayerMask.NameToLayer(Type))
                {
                    CanDissolve = true;
                }
                else
                {
                    CanDissolve = false;
                }
                return true;
            }
            else
            {
                Debug.DrawRay(RaycastPosition.position, Vector2.left * WallCheckDistance, Color.blue);
                return false;
            }
        }
        else
        {
            Debug.DrawRay(RaycastPosition.position, Vector2.left * WallCheckDistance, Color.blue);
            return false;
        }


    }

        public float GetEnergy()
    {
        return Energy;
    }

    [PunRPC] public void HandleBools()
    {
        //Handeling Booleans
        IsTouchingWall = IsWallLeft() || IsWallRight();
        CanDissolve = feet.IsTouchingLayers(LayerMask.GetMask(Type));
        CanSwim = gameObject.GetComponent<Collider2D>().IsTouchingLayers(GroundLayerMask);
        IsTouchingGround = feet.IsTouchingLayers(GroundLayerMask);
        anim.SetBool("Touching Ground", IsTouchingGround);
        anim.SetBool("Swimming", Swimming);
        if (WallRunning)
        {
            Hat.SetActive(false);
            if (IsWallLeft())
            {
                WallHatRight.SetActive(true);
                WallHatLeft.SetActive(false);
            }
            if (IsWallRight())
            {
                WallHatRight.SetActive(false);
                WallHatLeft.SetActive(true);
            }
        }
        else
        {
            Hat.SetActive(true);
            WallHatRight.SetActive(false);
            WallHatLeft.SetActive(false);
        }
    }

}


