using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class NameTag : MonoBehaviourPun
{
    [SerializeField] TextMeshProUGUI NameTagText;
    private void Start()
    {
        if (photonView.IsMine)
            NameTagText.text = PhotonNetwork.NickName;
        else
            NameTagText.text = photonView.Owner.NickName;
    }
}
