using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBar : MonoBehaviour
{
    [SerializeField] float maxScale = 1f, minScale = 0f;
    [SerializeField] PlayerShooting player;
    [SerializeField] float LerSpeed = 1f;
    public bool Horizontal = true;
    public bool Lerp = false;
    // Start is called before the first frame update
    void Start()
    {
              
    }

    // Update is called once per frame
    void Update()
    {
        if(!player)
            player = GameObject.Find("LocalPlayer").GetComponent<PlayerHolder>().playerController.GetComponent<PlayerShooting>();
        else
            ScaleFill();        
    }
    private void ScaleFill()
    {
        var scale = maxScale * player.GetPaintInTank();
        if (Lerp)
        {
            if (Horizontal)
                transform.localScale = new Vector3(Mathf.MoveTowards(transform.localScale.x, scale, Time.deltaTime * LerSpeed), transform.localScale.y, 1f);
            else
                transform.localScale = new Vector3(transform.localScale.x, Mathf.MoveTowards(transform.localScale.y, scale, Time.deltaTime * LerSpeed), 1f);
        }
        else
        {
            if (Horizontal)
                transform.localScale = new Vector3(scale, transform.localScale.y, 1f);
            else
                transform.localScale = new Vector3(transform.localScale.x, scale, 1f);
        }
    }
}
