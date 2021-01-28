using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CreateRoom : MonoBehaviourPunCallbacks
{

    public Text roomInput;



    public void createRoom()
    {
        string room = roomInput.text;
        if (room == "") { room = "NoName"; }
        PhotonNetwork.CreateRoom(room, new RoomOptions() { MaxPlayers = 4});

    }
}
