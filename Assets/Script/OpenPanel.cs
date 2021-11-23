using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    [SerializeField] Canvas openPanel;
    [SerializeField] Canvas CurrentCanvas;
    // Start is called before the first frame update
    public void Open_Panel()
    {
        CurrentCanvas.GetComponent<Canvas>().enabled = false;
        openPanel.gameObject.SetActive(true);
        openPanel.GetComponent<Canvas>().enabled = true;
        Invoke("CloseCurrentCanvas",Time.fixedDeltaTime);
    }
    public void CloseCurrentCanvas()
    {
        CurrentCanvas.gameObject.SetActive(false);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
