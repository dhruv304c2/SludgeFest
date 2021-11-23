using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SplatObject : MonoBehaviourPun
{
    [SerializeField] List<Sprite> SplatSprites;
    SpriteRenderer MySR;
    [SerializeField] float maxScale = 1.5f, minScale = 1f, DissolveTime = 60f;
    float DissolveTimer = 0f;
    bool Removing = false;
    [SerializeField] public string TeamType = "";
    // Start is called before the first frame update
    void Start()
    {
        MySR = GetComponent<SpriteRenderer>();
        int i = Random.Range(0, SplatSprites.Count);
        MySR.sprite = SplatSprites[i];
        float Scale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector2(Scale, Scale);
    }
    public IEnumerator Remove(float fadespeed = 0.008f)
    {
        if (!Removing)
        {
            Removing = true;
            var TargetColor = new Color(MySR.color.r, MySR.color.g, MySR.color.b, 0f);
            while (MySR.color.a != 0)
            {
                Fade(fadespeed);              
                yield return new WaitForEndOfFrame();
            }
            if (MySR.color.a == 0)
            {
                DestroySplat();               
            }
        }
    }
    private void Update()
    {
        DissolveTimer += Time.deltaTime;
        if(DissolveTimer>=DissolveTime && !Removing)
        {
            StartCoroutine(Remove());
        }
    }

    [PunRPC] public void Fade(float fadespeed = 0.008f)
    {
        if(MySR)
            MySR.color = new Color(MySR.color.r, MySR.color.g, MySR.color.b, Mathf.MoveTowards(MySR.color.a, 0f, fadespeed));
    }

    [PunRPC] public void DestroySplat()
    {
        Destroy(gameObject);
    }

    public string GetTeamType()
    {
        return TeamType;
    }
}
