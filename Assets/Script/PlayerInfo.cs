using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameText;
    void Update()
    {
        playerNameText.text = PlayerPrefs.GetString(PlayerNameInput.playerPrefKeyName);
    }
}
