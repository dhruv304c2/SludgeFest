using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListManager : MonoBehaviourPun
{
    [SerializeField] Transform BlueList;
    [SerializeField] Transform RedList;
    
    // Update is called once per frame
    void Start()
    {
        DevideTeamEqually();      
    }

    private void Update()
    {
        DevideTeamEqually();
    }
    public void AssignTeam(Player player,Transform Team,byte TeamNo)
    {
        int ActiveCount = 0;
        foreach(Transform PlayerListEntity in Team)
        {
            if(PlayerListEntity.gameObject.activeSelf)
            {
                ActiveCount++;
            }
        }
        Team.GetChild(ActiveCount).gameObject.SetActive(true);
        Team.GetChild(ActiveCount).gameObject.GetComponent<TextMeshProUGUI>().text = player.NickName;
        player.SetPlayerNumber(TeamNo);
    }
    
    public void DevideTeamEqually()
    {
        Player[] players;
        players = NetworkManager.GetPlayerList();
        foreach (Transform PlayerListItem in BlueList)
        {
            PlayerListItem.gameObject.SetActive(false);
        }
        foreach (Transform PlayerListItem in RedList)
        {
            PlayerListItem.gameObject.SetActive(false);
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (i % 2 == 0)
                AssignTeam(players[i], BlueList, 0);
            else
                AssignTeam(players[i], RedList, 1);
        }
    }
}
