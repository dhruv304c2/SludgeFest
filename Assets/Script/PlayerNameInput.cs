using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] Button okButton;
    [SerializeField] TMP_InputField playerInputField;

    [SerializeField] bool ChangeName;
    public static string playerPrefKeyName = "Player Name";
    // Start is called before the first frame update
    void Start()
    {
        okButton.interactable = !string.IsNullOrEmpty(playerInputField.text);
        if (!ChangeName && !string.IsNullOrEmpty(PlayerPrefs.GetString(playerPrefKeyName)))
        {
            okButton.GetComponent<OpenPanel>().Open_Panel();
        }
        playerInputField.text = PlayerPrefs.GetString(playerPrefKeyName);
    }

    public void SetPlayerName()
    {
        okButton.interactable = !string.IsNullOrEmpty(playerInputField.text);
    }

    public void SaveplayerName()
    {
        PlayerPrefs.SetString(playerPrefKeyName, playerInputField.text.ToUpper());
        NetworkManager.ChangePlayerTag(playerInputField.text.ToUpper());
    }
}
