using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject gameModeSelection;
    [SerializeField] GameObject mapSelection;
    [SerializeField] Text roomInput;
    [SerializeField] GameObject playerNumber;
    [SerializeField] Text SliderNumber;
    [SerializeField] PlayerListing PlayerListing;

    private byte numberOfPlayers = 2;
    private float gameMode = 0;
    private int map = 0;

    RoomOptions roomOpts = new RoomOptions();

    void Start()
    {
    }

    public void UpdateSlider()
    {
        numberOfPlayers = Convert.ToByte(playerNumber.GetComponent<Slider>().value);
        SliderNumber.text = playerNumber.GetComponent<Slider>().value.ToString();
    }

    public void UpdateDropDown()
    {
        gameMode = gameModeSelection.GetComponent<Dropdown>().value;
    }

    public void UpdateMapDropDown()
    {
        map = mapSelection.GetComponent<Dropdown>().value;
    }

    public void createRoom()
    {
        roomOpts.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();

        roomOpts.CustomRoomProperties.Add("GameMode", gameMode);
        roomOpts.CustomRoomProperties.Add("MaxPlayers", numberOfPlayers);
        roomOpts.CustomRoomProperties.Add("Map", map);

        string room = roomInput.text;
        if (room == "") { room = "NoName"; }
        PhotonNetwork.CreateRoom(room, roomOpts);

    }
}
