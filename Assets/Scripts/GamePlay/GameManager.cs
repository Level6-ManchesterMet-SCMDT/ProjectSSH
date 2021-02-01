using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;

    void Start()
    {
        // check whether this client already has a player setup
        if(PlayerManager.LocalPlayerInstance == null)
        {
            Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint();
            //if not, we need to make them one - spawn a character for the local player
            PhotonNetwork.Instantiate(this.playerPrefab.name, spawnpoint.position, spawnpoint.rotation, 0);
            
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

}
