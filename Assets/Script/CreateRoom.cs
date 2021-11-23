using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviour
{
    [SerializeField] TMP_InputField CreateRoomInputField;
    [SerializeField] Button okButton;
    public void Update()
    {
        okButton.interactable = !string.IsNullOrEmpty(CreateRoomInputField.text);
    }
    public void CreateRoomWithName()
    {
        NetworkManager.CreateRoom(CreateRoomInputField.text.ToUpper());
    }
}
