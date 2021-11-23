using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameMessages : MonoBehaviour
{
    static TextMeshProUGUI MessageText;
    static Animator anim;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
        MessageText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    public static IEnumerator ShowMessage(string Message,float Duration)
    {
        MessageText.text = Message;
        anim.Play("MessageEntry", 0);
        yield return new WaitForSeconds(Duration);
        anim.Play("MessageExit", 0);
    }
}
