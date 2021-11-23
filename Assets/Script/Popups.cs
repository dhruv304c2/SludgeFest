using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popups : MonoBehaviour
{
     public GameObject PopUpPanel;
     public TextMeshProUGUI HeadingText;
     public TextMeshProUGUI MessageText;

    public void ShowPopup(string Heading,string Message)
    {
        PopUpPanel.SetActive(true);
        HeadingText.text = Heading;
        MessageText.text = Message;
    }
    
    public void HidePopUp()
    {
        PopUpPanel.SetActive(false);
    }
}
