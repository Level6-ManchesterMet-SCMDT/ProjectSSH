using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public class TeamDeathmatch : GameModes
{
    [SerializeField] GameObject ScoreboardUpdater;

    public float maxTime;

    private float deathmatchTimer;
    private List<string> photonViewIDs;
    private float seconds = 0;
    private float minutes;
    private string decimals = "0";

    void Awake()
    {
        deathmatchTimer = maxTime;
        photonViewIDs = new List<string>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        MatchTimer();

        if (deathmatchTimer <= 0)
        {
            Debug.Log("Test1");
            foreach (string photonViewID in photonViewIDs)
            {
                PhotonView.Find(Int32.Parse(photonViewID)).gameObject.transform.Find("Canvas/UI").GetComponent<UpdateUI>().RoundOver();
            }
        }
        else
        {
            foreach (string photonViewID in photonViewIDs)
            {
                PhotonView.Find(Int32.Parse(photonViewID)).gameObject.transform.Find("Canvas/UI").GetComponent<UpdateUI>().UpdateTimer(deathmatchTimer, maxTime, seconds, minutes, decimals);
            }
        }
    }

    public override void RunGameMode(GameObject playerPrefab)
    {
        // check whether this client already has a player setup
        if (PlayerManager.LocalPlayerInstance == null)
        {
            Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint();

            //if not, we need to make them one - spawn a character for the local player
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnpoint.position, spawnpoint.rotation, 0);

            PhotonView photonView = PhotonView.Get(this);

            PhotonView playerPhotonView = PhotonView.Get(player);

            photonViewIDs.Add(playerPhotonView.ViewID.ToString());
        }
    }

    [PunRPC]
    void MatchTimer()
    {
        if (deathmatchTimer > 0)
        {
            deathmatchTimer -= Time.deltaTime;
        }

        if (deathmatchTimer > 59)
        {
            minutes = deathmatchTimer / 60;
            if (seconds <= 0)
            {
                seconds = 60;
            }
        }
        else
        {
            seconds = deathmatchTimer;
            minutes = 0;
        }

        if (seconds <= 0)
        {
            seconds = 0;
        }
        else seconds -= Time.deltaTime;

        if (seconds < 10 && seconds != 0)
        {
            decimals = "0";
        }
        else decimals = "";
    }
}