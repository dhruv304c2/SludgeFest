using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
public class ReloadScene : MonoBehaviourPun
{
    // Update is called once per frame

    [SerializeField] Button RematchButton;

    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            RematchButton.interactable = true;
        }
        else
        {
            RematchButton.interactable = false;
        }
    }

    public void reloadScene()
    {
        photonView.RPC("LoadSceneOnNetwork", RpcTarget.All);
    }

    [PunRPC] public void LoadSceneOnNetwork()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
