using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowItemCount : MonoBehaviour
{
    PlayerShooting player;
    TextMeshProUGUI ItemCountText;

    // Start is called before the first frame update
    void Start()
    {
        ItemCountText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            player = GameObject.Find("LocalPlayer").GetComponent<PlayerHolder>().playerController.GetComponent<PlayerShooting>();
        else
            ItemCountText.text = player.GetItemCount().ToString();
    }
}
