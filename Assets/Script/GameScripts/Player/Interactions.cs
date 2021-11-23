using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class Interactions : MonoBehaviourPun
{
    public bool Interact = false;
    [SerializeField] GameObject InteractPromptGraphic;
    [SerializeField] TextMeshProUGUI ActionText;
    // Start is called before the first frame update
    void Start()
    {
        if(!photonView.IsMine|| photonView.Owner != PhotonNetwork.LocalPlayer)
        {
            Destroy(GetComponent<Interactions>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2"))
        {
            Interact = true;
        }
        else
        {
            Interact = false;
        }
    }

    public void HideInteractGraphic()
    {
        InteractPromptGraphic.SetActive(false);        
    }

    public  void ShowInteractGraphic()
    {
        InteractPromptGraphic.SetActive(true);
    }

    public void ActionMessage(string Message)
    {
        ActionText.text = Message;
    }
}
