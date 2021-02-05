using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TeamDeathmatch : GameModes
{
    public float maxTime = 180;
    private List<GameObject> players;

    public override void RunGameMode(GameObject playerPrefab)
    {
        // check whether this client already has a player setup
        if (PlayerManager.LocalPlayerInstance == null)
        {
            Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint();
            //if not, we need to make them one - spawn a character for the local player
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnpoint.position, spawnpoint.rotation, 0);
            //players.Add(playerPrefab);
            UISetup(player);
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    void UISetup(GameObject player)
    {
        player.transform.Find("Canvas/UI").GetComponent<UpdateUI>().UpdateValues(maxTime, "TeamDeathMatch");
        Debug.LogFormat("Player", player.transform);
        //Debug.Log("UISETUP " + playerPrefab.transform.Find("Canvas/UI").GetComponent<UpdateUI>().hasUpdated);
    }
}