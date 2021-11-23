using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveMarkerUI : MonoBehaviour
{
    [SerializeField] CapturePoint capturePoint;
    [SerializeField] GameObject MarkerGameobject;
    [SerializeField] SpriteRenderer MarkerGraphic;
    [SerializeField] TextMeshPro Text;
    [SerializeField] float Padding = 100f;
    [SerializeField] List<Sprite> IndicatorSprites;
    [SerializeField] float MaxDistanceToShow = 300f;
    public PlayerMovement player;
    public Camera PlayerCam;
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

        if (IsOffScreen())
        {
            var ScreenPoint = PlayerCam.WorldToScreenPoint(capturePoint.trackPoint.position);
            if (ScreenPoint.x < 0 + Padding) ScreenPoint.x = 0 + Padding;
            if (ScreenPoint.x > Screen.width - Padding) ScreenPoint.x = Screen.width - Padding;
            if (ScreenPoint.y < 0 + Padding) ScreenPoint.y = 0 + Padding;
            if (ScreenPoint.y > Screen.height - Padding) ScreenPoint.y = Screen.height - Padding;
            MarkerGameobject.transform.position = PlayerCam.ScreenToWorldPoint(ScreenPoint);
            MarkerGameobject.transform.position = new Vector3(MarkerGameobject.transform.position.x, MarkerGameobject.transform.position.y, 0f);
            Rotatemarker();
        }
        else
        {
            MarkerGameobject.transform.position = capturePoint.trackPoint.position;
            MarkerGraphic.gameObject.transform.rotation = Quaternion.Euler(0, 0, 180f);
        }
        HandleSprite();
        if(Vector2.Distance(player.transform.position,capturePoint.trackPoint.position) >= MaxDistanceToShow)
        {
            MarkerGraphic.enabled=false;
            Text.enabled = false;
        }
        else
        {
            MarkerGraphic.enabled=true;
            Text.enabled = true;
        }
    }
    private void Rotatemarker()
    {
        Vector2 RelativeVector = (capturePoint.trackPoint.position - player.transform.position).normalized;
        float Angle = Mathf.Atan2(RelativeVector.y, RelativeVector.x) * Mathf.Rad2Deg - 90f;
        MarkerGraphic.gameObject.transform.rotation = Quaternion.Euler(0, 0, Angle);
    }

    public bool IsOffScreen()
    {
        var ScreenPoint = PlayerCam.WorldToScreenPoint(capturePoint.trackPoint.position);
        return ScreenPoint.x < 0 + Padding || ScreenPoint.x > Screen.width - Padding || ScreenPoint.y < 0 + Padding || ScreenPoint.y > Screen.height - Padding;
    }

    public void HandleSprite()
    {
        switch (capturePoint.ControllingTeam)
        {
            case "Blue":
                MarkerGraphic.sprite = IndicatorSprites[1];
                break;
            case "Red":
                MarkerGraphic.sprite = IndicatorSprites[2];
                break;
            default:
                MarkerGraphic.sprite = IndicatorSprites[0];
                break;
        }
    }
}
