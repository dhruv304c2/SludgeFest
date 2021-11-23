using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillingStation : MonoBehaviour
{
    [SerializeField] float CoolDown;
    [SerializeField] CapturePoint capturePoint;

    private float Timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {
        if (Timer < CoolDown)
        { Timer += Time.deltaTime; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Interactions>())
        {
            if (Timer >= CoolDown && capturePoint.ControllingTeam == collision.GetComponent<PlayerMovement>().Type)
            {
                collision.GetComponent<Interactions>().ShowInteractGraphic();
                collision.GetComponent<Interactions>().ActionMessage("Fill Tank");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactions>())
        {
            if (Timer >= CoolDown && capturePoint.ControllingTeam == collision.GetComponent<PlayerMovement>().Type)
                collision.GetComponent<Interactions>().HideInteractGraphic();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Interactions>())
        {
            if(collision.GetComponent<Interactions>().Interact)
            {
                if(Timer>=CoolDown && capturePoint.ControllingTeam == collision.GetComponent<PlayerMovement>().Type)
                {
                    collision.GetComponent<PlayerShooting>().PaintinTank = 1f;
                    Timer = 0f;
                }
            }
            if (Timer >= CoolDown && capturePoint.ControllingTeam == collision.GetComponent<PlayerMovement>().Type)
            {
                collision.GetComponent<Interactions>().ShowInteractGraphic();
                collision.GetComponent<Interactions>().ActionMessage("Fill Tank");
            }
            else
            {
                collision.GetComponent<Interactions>().HideInteractGraphic();
            }
        }
    }
}
