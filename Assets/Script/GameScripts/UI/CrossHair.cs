using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    Camera PlayerCam;
    private PlayerMovement player;
    [SerializeField] float DampDistance= 0.1f;
    [SerializeField] float LerpSeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            foreach (PlayerMovement playerMovement in FindObjectsOfType<PlayerMovement>())
            {
                if (playerMovement.photonView.IsMine)
                {
                    player = playerMovement;
                    PlayerCam = player.PlayerCam;
                }
            }
        }
        if (Vector3.Distance(transform.position, PlayerCam.ScreenToWorldPoint(Input.mousePosition)) <= DampDistance)
            transform.position = Vector3.MoveTowards(transform.position, PlayerCam.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * LerpSeed);
        else
            transform.position = PlayerCam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    } 
}
