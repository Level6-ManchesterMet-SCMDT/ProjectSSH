using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public GameObject GameModesContainer;



    private List<GameModes> gameModeList;
    private GameModes currentGameMode;
    private int gameModeNumber;

    void Awake()
    {
        gameModeList = new List<GameModes>();

        foreach (GameModes element in GameModesContainer.GetComponents(typeof(GameModes))) //load the Gamemodes Scripts into a list
        {
            if (element != null)
            {
                gameModeList.Add(element);
            }
        }
    }

    void Start()
    {
        gameModeNumber = Int32.Parse(PhotonNetwork.CurrentRoom.CustomProperties["GameMode"].ToString());


    }

    void Update()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties["GameMode"] != null)
        {
            currentGameMode = gameModeList[gameModeNumber];
            currentGameMode.RunGameMode(playerPrefab);
        }
    }
}
