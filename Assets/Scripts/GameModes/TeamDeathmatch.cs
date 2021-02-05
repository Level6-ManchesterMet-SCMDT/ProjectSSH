using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeamDeathmatch : GameModes
{
    public float maxTime;

    private float deathmatchTimer;
    private List<string> playerNames;
    private List<string> playerKills;
    private List<string> playerDeaths;
    private List<GameObject> players;

    void Start()
    {
        players = new List<GameObject>();
        playerNames = new List<string>();
        playerKills = new List<string>();
        playerDeaths = new List<string>();

        deathmatchTimer = maxTime;

    }

    void Update()
    {
        if (deathmatchTimer > 0)
        {
            deathmatchTimer -= Time.deltaTime;
            
        }
        else
        {
            foreach(GameObject player in players)
            {
                player.transform.Find("Canvas/UI").GetComponent<UpdateUI>().RoundOver();
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

            players.Add(player);

            playerNames.Add(PhotonNetwork.NickName);
            playerKills.Add(PhotonNetwork.LocalPlayer.CustomProperties["Kills"].ToString());
            playerDeaths.Add(PhotonNetwork.LocalPlayer.CustomProperties["Deaths"].ToString());
            
            UISetup(player);
        }
        else
        {
           // Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    void UISetup(GameObject player)
    {
        player.transform.Find("Canvas/UI").GetComponent<UpdateUI>().UpdateValues(maxTime, "TeamDeathMatch");
        player.transform.Find("Canvas/UI").GetComponent<UpdateUI>().UpdateScores(playerNames, playerKills, playerDeaths, "test");
    }
}