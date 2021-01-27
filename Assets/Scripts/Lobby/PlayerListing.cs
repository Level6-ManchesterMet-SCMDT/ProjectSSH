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

    public GameObject PlayerIdleCard;

    private ExitGames.Client.Photon.Hashtable Cproperties = new ExitGames.Client.Photon.Hashtable();

    public GameObject readyBut;

    private Player[] players;
    private Dictionary<int, GameObject> playerList;

    int pCount = 0;
    public void Start()
    {
        players = PhotonNetwork.PlayerList;
        playerList = new Dictionary<int, GameObject>();
        updatePlayerList();



        //foreach (Player p in PhotonNetwork.PlayerList)
        //{
        //    GameObject card = PhotonNetwork.Instantiate(PlayerCard.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        //    card.transform.SetParent(gameObject.transform);
        //    card.transform.localScale = Vector3.one;
        //    card.GetComponent<PlayerCard>().setValue(p.NickName, p.ActorNumber.ToString());
        //    playerList.Add(p.ActorNumber, card);
        //}



        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject card = Instantiate(PlayerIdleCard, new Vector3(0f, 0f, 0f), Quaternion.identity);

            //GameObject card = PhotonNetwork.Instantiate(PlayerIdleCard.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
            card.transform.SetParent(pCardSpawners[p.ActorNumber - 1].transform);
            card.transform.localScale = Vector3.one;
            card.transform.localPosition = new Vector3(0f, 0f, 0f);
            card.transform.localRotation = Quaternion.identity;
            card.GetComponent<PlayerCard>().setValue(p.NickName);//, p.ActorNumber.ToString());
            playerList.Add(p.ActorNumber, card);
            //pCount++;
            
        }
    }



    public override void OnPlayerLeftRoom(Player other)
    {
        Destroy(playerList[other.ActorNumber].gameObject);
        playerList.Remove(other.ActorNumber);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Debug.Log("Enter");
        //GameObject card = PhotonNetwork.Instantiate(PlayerCard.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        //card.transform.SetParent(gameObject.transform);
        //card.transform.localScale = Vector3.one;
        //card.GetComponent<PlayerCard>().setValue(newPlayer.NickName, newPlayer.ActorNumber.ToString());
        //playerList.Add(newPlayer.ActorNumber, card);

        GameObject card = Instantiate(PlayerIdleCard, new Vector3(0f, 0f, 0f), Quaternion.identity);

        //GameObject card = PhotonNetwork.Instantiate(PlayerIdleCard.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        card.transform.SetParent(pCardSpawners[newPlayer.ActorNumber - 1].transform);
        card.transform.localScale = Vector3.one;
        card.transform.localPosition = new Vector3(0f, 0f, 0f);
        card.transform.localRotation = Quaternion.identity;
        card.GetComponent<PlayerCard>().setValue(newPlayer.NickName);//, p.ActorNumber.ToString());
        playerList.Add(newPlayer.ActorNumber, card);
        //pCount++;
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
        //Debug.Log(PhotonNetwork.CurrentRoom);
        //Debug.Log(PhotonNetwork.IsMasterClient);

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
        PhotonNetwork.SetPlayerCustomProperties(Cproperties);
        readyBut.SetActive(false);
    }

}
