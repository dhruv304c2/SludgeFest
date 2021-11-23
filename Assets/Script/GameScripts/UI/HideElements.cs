using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HideElements : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Awake()
    {
        if (photonView.IsMine) { return; }
        else
        {
            Destroy(gameObject);
        }
    }
}
