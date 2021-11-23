using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public static NetworkManager instance;

    [SerializeField] public static byte MaxPlayerPerRoom = 10;

    public Popups PopUpManager;
    private void Awake()
    {
        if (instance != null && instance != this)
            gameObject.SetActive(false);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        PhotonNetwork.AutomaticallySyncScene = true;
        PopUpManager = FindObjectOfType<Popups>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
    }
    public void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.NickName = PlayerPrefs.GetString(PlayerNameInput.playerPrefKeyName);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created:" + PhotonNetwork.CurrentRoom.Name);
        SceneManager.LoadScene(1);
    }

    public static void CreateRoom(string roomName)
    {
        PhotonNetwork.CreateRoom(roomName,new RoomOptions { MaxPlayers = MaxPlayerPerRoom});
    }

    public static void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        PopUpManager.ShowPopup("Error!!!", message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PopUpManager.ShowPopup("Error!!!", message);
    }

    public static void ChangePlayerTag(string PlayerName)
    {
        PhotonNetwork.NickName = PlayerName;        
    }

    public static Player[] GetPlayerList()
    {
        return PhotonNetwork.PlayerList;
    }

    public static void CloseRoom()
    {
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.ConnectUsingSettings();
        SceneManager.LoadScene(0);
    }

}
