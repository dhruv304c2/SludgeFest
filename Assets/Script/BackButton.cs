using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    [SerializeField] int BuildSceneindex;

    public void LoadScene()
    {
        SceneManager.LoadScene(BuildSceneindex);
    }
    public void CloseRoom()
    {
        NetworkManager.CloseRoom();
    }
}
