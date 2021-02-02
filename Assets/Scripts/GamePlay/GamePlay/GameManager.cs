using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab;
    public int myTeam;

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

/*    [PunRPC]
    void RPC_GetTeam()
    {
        myTeam = SpawnManager.nextPlayersTeam;
        SpawnManager.UpdateTeam();
        PV.RPC("RPC_SentTeam", RpcTarget.OtherBuffered, myTeam);
    }

    [PunRPC]
    void RPC_SentTeam(int whichTeam)
    {
        myTeam = whichTeam;
    }*/
}
