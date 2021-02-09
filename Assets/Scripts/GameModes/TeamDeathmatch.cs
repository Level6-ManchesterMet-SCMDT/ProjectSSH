using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public class TeamDeathmatch : GameModes
{
    public float maxTime;

    private float deathmatchTimer;

    private List<string> photonViewIDs;
    //private string[] newPhotonViewIDs = new string[0];
    //private List<string> playerNames;
    //private string[] newPlayerNames = new string[0];

    [SerializeField] GameObject ScoreboardUpdater;

    private float seconds = 0;
    private float minutes;
    private string decimals = "0";
    //private int photonviewID;

    void Awake()
    {
        deathmatchTimer = maxTime;
        photonViewIDs = new List<string>();
        //playerNames = new List<string>();
        //newPhotonViews = new int[0]();

        //PhotonNetwork.Instantiate(ScoreboardUpdater.name, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity, 0);

    }

    void Update()
    {
        MatchTimer();

        if (deathmatchTimer <= 0)
        {
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

            //photonViewIDs = newPhotonViewIDs.OfType<string>().ToList();
            photonViewIDs.Add(playerPhotonView.ViewID.ToString());
            //newPhotonViewIDs = photonViewIDs.ToArray();
/*
            playerNames = newPlayerNames.OfType<string>().ToList();
            playerNames.Add(playerPhotonView.Owner.NickName);
            newPlayerNames = playerNames.ToArray();*/

           // photonView.RPC("RPC_PlayerInstantiated", RpcTarget.All, string.Join("\r", newPhotonViewIDs));
        }

        
    }


    /* void RPC_PlayerInstantiated(string playerViewIDs)
     {
         string[] playerViewsArray = playerViewIDs.Split('\r');

         foreach (string playerViewID in playerViewsArray)
         {
             PhotonView.Find(Int32.Parse(playerViewID)).gameObject.transform.Find("Canvas").Find("UI").GetComponent<UpdateUI>().playerInstantiated(playersArray);
         }*/

    /*
    var photonViews = UnityEngine.Object.FindObjectsOfType<PhotonView>();

    foreach (var view in photonViews)
    {
        view.gameObject.transform.Find("Canvas").Find("UI").GetComponent<UpdateUI>().playerInstantiated(playersArray);
    }*/
    // }

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

/*    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(newPhotonViewIDs);
            stream.SendNext(newPlayerNames);
        }
        else
        {
            this.newPhotonViewIDs = (string[])stream.ReceiveNext();
            this.newPlayerNames = (string[])stream.ReceiveNext();
        }
    }*/
}