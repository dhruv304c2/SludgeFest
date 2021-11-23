using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grenade : MonoBehaviourPun
{
    [SerializeField] ParticleSystem Particles;
    [SerializeField] float FuseTimer;
    [SerializeField] string Color;
    [SerializeField] float Damage;
    [SerializeField] PlayerMovement LocalPlayer;
    [SerializeField] float MaxCameraShakeDistance = 30f;
    [SerializeField] float SurfaceCheckDistance = 1f;
    [SerializeField] LayerMask SurfaceCheckMask;
    [SerializeField] WallPaint wallPaint;
    [SerializeField] Color WallPaintColor;
    [SerializeField] List<GameObject> Effects;
    bool explode = false;
    List<float> Angles;

    public float ThrowingVelocity;

    private List<GameObject> ObjectsInRadius = new List<GameObject>();

    private void Awake()
    {
        float[] AllowedAngles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        Angles = new List<float>(AllowedAngles);
    }
    // Start is called before the first frame update
    IEnumerator Start()
    {
        LocalPlayer = FindObjectOfType<PlayerMovement>();
        yield return new WaitForSeconds(FuseTimer);
        Explode();
    }

    [PunRPC] public void Explode()
    {
        Particles.transform.parent = null;
        Particles.Play();
        explode = true;
        PlayEffects();
        FormSplatter(transform.position);
        if(photonView.IsMine)
        {
            foreach(GameObject other in ObjectsInRadius)
            {

                if (photonView.IsMine)
                {
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
                            other.GetComponent<PlayerHealth>().DealDamageOnNetwork(Damage);
                        }
                    }
                }
            }           
        }
        if(Vector2.Distance(transform.position,LocalPlayer.transform.position)< MaxCameraShakeDistance )
        {
            CameraShake.cameraShake(5f, 0.5f);
        }
        Destroy(GetComponent<SpriteRenderer>());
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ObjectsInRadius.Add(collision.gameObject);       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        ObjectsInRadius.Remove(collision.gameObject);
    }

    public void FormSplatter(Vector2 Position)
    {
        RaycastHit2D LeftSurfaceCheck = Physics2D.Raycast(transform.position, Vector2.left,SurfaceCheckDistance,SurfaceCheckMask);
        RaycastHit2D RightSurfaceCheck = Physics2D.Raycast(transform.position, Vector2.right, SurfaceCheckDistance, SurfaceCheckMask);
        RaycastHit2D UpSurfaceCheck = Physics2D.Raycast(transform.position, Vector2.up, SurfaceCheckDistance, SurfaceCheckMask);
        RaycastHit2D DownSurfaceCheck = Physics2D.Raycast(transform.position, Vector2.down, SurfaceCheckDistance, SurfaceCheckMask);

        if (LeftSurfaceCheck)
        {
            float[] Remove = { 180f, 135f, 225f };
            RemoveAngles(Remove);            
        }
        if (RightSurfaceCheck)
        {
            float[] Remove = { 0f,45f,315f };
            RemoveAngles(Remove);
        }
        if (UpSurfaceCheck)
        {
            float[] Remove = { 90f, 45f, 135f };
            RemoveAngles(Remove);
        }
        if (DownSurfaceCheck)
        {
            float[] Remove = { 270f, 225f, 315f };
            RemoveAngles(Remove);
        }
        foreach (float angle in Angles)
        {
            GameObject WallPaint = Instantiate(wallPaint.gameObject, Position, Quaternion.Euler(0, 0, angle));
            WallPaint.GetComponent<SpriteRenderer>().color = WallPaintColor;
        }
    }

    private void RemoveAngles(float[] AnglesToRemove)
    {
        List<float> RemoveAngles = new List<float>(AnglesToRemove);
        foreach (float Angle in RemoveAngles)
        {
            Angles.Remove(Angle);
        }
    }

    private void PlayEffects()
    {
        foreach(GameObject Fx in Effects)
        {
            Instantiate(Fx, transform.position, Quaternion.identity);
        }
    }
}
