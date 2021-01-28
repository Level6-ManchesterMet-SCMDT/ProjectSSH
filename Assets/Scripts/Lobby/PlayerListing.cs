using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
//using ExitGames.Client.Photon;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    public Text lobbynames;

    public GameObject PlayerCard;
    public GameObject startGameButton;
    //public GameObject readyBut;

    private Player[] players;
    private Dictionary<int, GameObject> playerList;

    
    public void Start()
    {
        players = PhotonNetwork.PlayerList;
        playerList = new Dictionary<int, GameObject>();
        updatePlayerList();



        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject card = PhotonNetwork.Instantiate(PlayerCard.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
            card.transform.SetParent(gameObject.transform);
            card.transform.localScale = Vector3.one;
            card.GetComponent<PlayerCard>().setValue(p.NickName, p.ActorNumber.ToString());
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
        Debug.Log("Enter");
        GameObject card = PhotonNetwork.Instantiate(PlayerCard.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        card.transform.SetParent(gameObject.transform);
        card.transform.localScale = Vector3.one;
        card.GetComponent<PlayerCard>().setValue(newPlayer.NickName, newPlayer.ActorNumber.ToString());
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
        Debug.Log(PhotonNetwork.CurrentRoom);
        Debug.Log(PhotonNetwork.IsMasterClient);

        //lobbynames.text = PhotonNetwork.CurrentRoom.Name;
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.SetActive(true);
        }
    }



    //public void readyUp()
    //{
    //    bool ready = true;
    //    Cproperties[GeomTuneGame.Player_Ready] = ready;
    //    PhotonNetwork.SetPlayerCustomProperties(Cproperties);
    //    readyBut.SetActive(false);
    //}

}
