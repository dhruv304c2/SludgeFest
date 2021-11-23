using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeathBar : MonoBehaviour
{
    [SerializeField] float maxScale = 1f, minScale = 0f;
    [SerializeField] PlayerHealth player;
    [SerializeField] SpriteRenderer BackGround;
    [SerializeField] SpriteRenderer Fill;
    [SerializeField] float ShowFadeSpeed = 0.25f;
    [SerializeField] float HideFadeSpeed = 0.25f;
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
        if (!player)
            player = GameObject.Find("LocalPlayer").GetComponent<PlayerHolder>().playerController.GetComponent<PlayerHealth>();
        else
            ScaleFill();
        if (BackGround && Fill)
        {
            if (player.GetHealth() == 1f)
            {
                HideBar();
            }
            else
            {
                ShowBar();
            }
        }
    }
    private void ScaleFill()
    {
        var scale = maxScale * player.GetHealth();
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
    public void ShowBar()
    {
        BackGround.color = new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b, Mathf.Lerp(BackGround.color.a, 255f, ShowFadeSpeed * Time.deltaTime));
        Fill.color = new Color(Fill.color.r, Fill.color.g, Fill.color.b, Mathf.Lerp(Fill.color.a, 255f, ShowFadeSpeed * Time.deltaTime));
    }
    public void HideBar()
    {
        BackGround.color = new Color(BackGround.color.r, BackGround.color.g, BackGround.color.b, Mathf.Lerp(BackGround.color.a, 0f, ShowFadeSpeed * Time.deltaTime));
        Fill.color = new Color(Fill.color.r, Fill.color.g, Fill.color.b, Mathf.Lerp(Fill.color.a, 0f, ShowFadeSpeed * Time.deltaTime));
    }
}
