using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    public Text lobbynames;

    public GameObject PlayerCard;
    public GameObject startGameButton;
    public Transform[] pCardSpawners;
    public int currentPlayerPose;

    public GameObject PlayerIdleCard;

    private ExitGames.Client.Photon.Hashtable Cproperties = new ExitGames.Client.Photon.Hashtable();

    public GameObject readyBut;

    private Player[] players;
    private Dictionary<int, GameObject> playerList;

    public void Start()
    {
        players = PhotonNetwork.PlayerList;
        playerList = new Dictionary<int, GameObject>();
        updatePlayerList();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject card = Instantiate(PlayerIdleCard, new Vector3(0f, 0f, 0f), Quaternion.identity);
            card.GetComponent<Animator>().SetInteger("RandomAnimation", currentPlayerPose);
            card.transform.position = pCardSpawners[currentPlayerPose].transform.position;
            card.transform.rotation = pCardSpawners[currentPlayerPose].transform.rotation;
            card.transform.GetChild(0).gameObject.SetActive(true);
            card.GetComponent<PlayerCard>().setValue(p.NickName);
            playerList.Add(p.ActorNumber, card);
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Destroy(playerList[other.ActorNumber].gameObject);
        playerList.Remove(other.ActorNumber);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        /*        GameObject card = Instantiate(PlayerIdleCard, new Vector3(0f, 0f, 0f), Quaternion.identity);

                card.transform.SetParent(pCardSpawners[newPlayer.ActorNumber - 1].transform);
                card.transform.localScale = Vector3.one;
                card.transform.localPosition = new Vector3(0f, 0f, 0f);
                card.transform.localRotation = Quaternion.identity;
                card.GetComponent<PlayerCard>().setValue(newPlayer.NickName);
                playerList.Add(newPlayer.ActorNumber, card);*/

        GameObject card = Instantiate(PlayerIdleCard, new Vector3(0f, 0f, 0f), Quaternion.identity);
        card.GetComponent<Animator>().SetInteger("RandomAnimation", currentPlayerPose);
        card.transform.position = pCardSpawners[currentPlayerPose].GetChild(newPlayer.ActorNumber - 1).transform.position;
        card.transform.rotation = pCardSpawners[currentPlayerPose].GetChild(newPlayer.ActorNumber - 1).transform.rotation;
        card.transform.GetChild(0).gameObject.SetActive(true);
        card.GetComponent<PlayerCard>().setValue(newPlayer.NickName);
        playerList.Add(newPlayer.ActorNumber, card);
    }

    public void startGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            players = PhotonNetwork.PlayerList;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if ((bool)p.CustomProperties["Ready"] == false)
                {
                    return;
                }
            }

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("DemoTesting");
        }
    }


    private void updatePlayerList()
    {
        lobbynames.text = PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
    }



    public void readyUp()
    {
        bool ready = true;
        Cproperties["Ready"] = ready;
        Cproperties["Deaths"] = 0;
        Cproperties["Kills"] = 0;
        Cproperties["AbilityPoints"] = 0;
        Cproperties["Currency"] = 0;
        PhotonNetwork.SetPlayerCustomProperties(Cproperties);
        readyBut.SetActive(false);
    }

}
