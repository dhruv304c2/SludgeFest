using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPun
{
    [SerializeField] PlayerHolder PlayerPrefabBlue;
    [SerializeField] PlayerHolder PlayerPrefabRed;
    [SerializeField] Transform RedSpawnPoint;
    [SerializeField] Transform BlueSpawnPoint;
    [SerializeField] SplatHolder SplatHolder;

    private void Start()
    {
        PlayerSpawn();
    }
    
    public void PlayerSpawn()
    {
        if (byte.Equals(PhotonNetwork.LocalPlayer.GetPlayerNumber(), 0))
        {
            var player = PhotonNetwork.Instantiate(PlayerPrefabBlue.name, BlueSpawnPoint.position, Quaternion.identity);
            player.name = "LocalPlayer";
        }
        else if (byte.Equals(PhotonNetwork.LocalPlayer.GetPlayerNumber(), 1))
        { 
            var player = PhotonNetwork.Instantiate(PlayerPrefabRed.name, RedSpawnPoint.position, Quaternion.identity);
            player.name = "LocalPlayer";
        }
        
        PhotonNetwork.Instantiate(SplatHolder.name, transform.position, Quaternion.identity);
    }
}
