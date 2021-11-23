using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundPointsCounter : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI RedTeamText;
    [SerializeField] TextMeshProUGUI BlueTeamText;

    public int BlueTeamPoints = 0;
    public int  RedTeamPoints = 0;

    [SerializeField] int WinLimit = 100;
    [SerializeField] float CountPointsInSeconds = 1f;
    [SerializeField] int pointsPerCP = 1;

    [SerializeField] Canvas VictoryCanvas;
    [SerializeField] TextMeshProUGUI WinnersName;
    float timer = 0f;
    public CapturePoint[] capturePoints;

    private void Start()
    {
        capturePoints = FindObjectsOfType<CapturePoint>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= CountPointsInSeconds)
        {
            timer = 0;
            foreach(CapturePoint capturePoint in capturePoints)
            {
                if (capturePoint.ControllingTeam == "Blue")
                {
                    photonView.RPC("IncrementBlue", RpcTarget.All);                   
                }
                if (capturePoint.ControllingTeam == "Red")
                {
                    photonView.RPC("IncrementRed", RpcTarget.All);
                }
            }
        }

        if(BlueTeamPoints >= WinLimit)
        {
            VictoryCanvas.gameObject.SetActive(true);
            WinnersName.text = "Blue";
            Cursor.visible = true;
        }
        if (RedTeamPoints >= WinLimit)
        {
            VictoryCanvas.gameObject.SetActive(true);
            WinnersName.text = "Red";
            Cursor.visible = true;
        }
    }

    [PunRPC] public void IncrementBlue()
    {
        BlueTeamPoints+=pointsPerCP;
        BlueTeamText.text = BlueTeamPoints.ToString();
    }
    
    [PunRPC] public void IncrementRed()
    {
        RedTeamPoints+=pointsPerCP;
        RedTeamText.text = RedTeamPoints.ToString();
    }
}
