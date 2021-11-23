using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayerManger : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        PlayerMovement[] playerList = FindObjectsOfType<PlayerMovement>();
        foreach (PlayerMovement player in playerList)
        {
            if(player.photonView.Owner.GetPlayerNumber() != PhotonNetwork.LocalPlayer.GetPlayerNumber())
            {
                player.gameObject.layer = LayerMask.NameToLayer("EnemyPlayer");
            }
        }
    }
}
