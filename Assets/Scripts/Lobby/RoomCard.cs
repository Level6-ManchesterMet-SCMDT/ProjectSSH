using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomCard : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public Text roomName;
    public Text pCount;
    public Button joinButton;


    public void setValues(string RoomName, int playerCount)
    {
        roomName.text = RoomName;
        pCount.text = playerCount + "/4";
    }

    public void OnJoinButtonClick()
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }
}
