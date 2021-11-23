using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JoinRoom : MonoBehaviour
{
    [SerializeField] TMP_InputField JoinRoomInputField;
    [SerializeField] Button okButton;
    public void Update()
    {
        okButton.interactable = !string.IsNullOrEmpty(JoinRoomInputField.text);
    }
    public void JoinRoomWithName()
    {
        NetworkManager.JoinRoom(JoinRoomInputField.text.ToUpper());
    }
}
