using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureProgressScript : MonoBehaviour
{
    [SerializeField] float maxScale = 1f, minScale = 0f;
    [SerializeField] CapturePoint capturePoint;
    [SerializeField] SpriteRenderer BackGround;
    [SerializeField] SpriteRenderer Fill;
    [SerializeField] float ShowFadeSpeed = 0.25f;
    [SerializeField] float HideFadeSpeed = 0.25f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ScaleFill();
        if (BackGround && Fill)
        {
            if (!capturePoint.UnderAttack)
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
        var xscale = maxScale * capturePoint.GetAttackProgress();
        transform.localScale = new Vector3(xscale, transform.localScale.y, 1f);
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
