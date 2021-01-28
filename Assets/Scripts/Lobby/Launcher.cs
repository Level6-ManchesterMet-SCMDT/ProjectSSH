using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1";

    public GameObject connectPanel;
    public GameObject homePanel;
    public GameObject lobbyPanel;

    //public Text input;
    //public Text roomInput;
    //public Text ConnectText;

    //public GameObject MakeRoom;
    //public GameObject roomCard;
    //public GameObject roomListing;

    //private ExitGames.Client.Photon.Hashtable Cproperties = new ExitGames.Client.Photon.Hashtable();

    //private Dictionary<string, GameObject> roomListingCards;
    void Awake()
    {
        //roomListingCards = new Dictionary<string, GameObject>();
        //PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.AutomaticallySyncScene = true;


        //bool ready = false;
        //Cproperties[] = ready;
        //PhotonNetwork.SetPlayerCustomProperties(Cproperties);
        //Cproperties[GeomTuneGame.Player_Lives] = GeomTuneGame.MAX_LIVES;
        //PhotonNetwork.SetPlayerCustomProperties(Cproperties);
    }


    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by pun");
        PhotonNetwork.JoinLobby();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);

    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Connected");
        connectPanel.SetActive(false);
        homePanel.SetActive(true);

    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() was called by pun. This client has joined a room");
        lobbySwitch();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("hello");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by pun. No random room avaliable");
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
        //base.OnCreateRoomFailed(returnCode, message);
    }
    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Lobby");
    }

    public void lobbySwitch()
    {
        homePanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

}
