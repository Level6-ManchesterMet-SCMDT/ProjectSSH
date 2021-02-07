using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UpdateUI : MonoBehaviourPunCallbacks
{
    public GameObject Timer;
    public GameObject Player;
    public GameObject[] Score;

    private string currentGameMode;
    private List<string> playerNames;
    private List<int> playerKills;
    private List<int> playerDeaths;

    void Awake()
    {
        playerNames = new List<string>();
        playerKills = new List<int>();
        playerDeaths = new List<int>();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            int n = p.ActorNumber - 1;

            playerKills.Add(0);
            playerDeaths.Add(0);

            playerNames.Add(p.NickName);
            Score[n].SetActive(true);
            Score[n].transform.GetChild(0).GetComponent<Text>().text = playerNames[n];
            Score[n].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = playerKills[n].ToString();
            Score[n].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = playerDeaths[n].ToString();
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
/*        playerNames.Remove(other.NickName);
        Score[other.ActorNumber].SetActive(false);
        Score[other.ActorNumber].transform.GetChild(0).GetComponent<Text>().text = playerNames[other.ActorNumber];
        Score[other.ActorNumber].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = playerKills[other.ActorNumber].ToString();
        Score[other.ActorNumber].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = playerDeaths[other.ActorNumber].ToString();*/
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerNames.Add(newPlayer.NickName);
        int n = newPlayer.ActorNumber - 1;
        Score[n].SetActive(true);
        Score[n].transform.GetChild(0).GetComponent<Text>().text = playerNames[n];
        Score[n].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = playerKills[n].ToString();
        Score[n].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = playerDeaths[n].ToString();
    }


    void Update()
    {

    }

/*    public void playerInstantiated(string[] intantiatedPlayerNames)
    {
        UpdateScores();
    }*/

    public void RoundOver()
    {
        this.transform.GetChild(4).gameObject.SetActive(true);
    }

    public void PlayerDied(string playerDied, string playerKiller)
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            if(playerDied == playerNames[i])
            {
                playerDeaths[i]++;
                UpdateScores();
            }

            if (playerKiller == playerNames[i])
            {
                playerKills[i]++;
                UpdateScores();
            }
        }
    }

    public void UpdateScores()
    {
        for(int i = 0; i < playerNames.Count; i++)
        {
            Score[i].transform.GetChild(1).GetChild(0).GetComponent<Text>().text = playerKills[i].ToString();
            Score[i].transform.GetChild(2).GetChild(0).GetComponent<Text>().text = playerDeaths[i].ToString();
        }
    }

    public void UpdateTimer(float timeRemaining, float totalTime, float seconds, float minutes, string decimals)
    {
        Timer.transform.GetChild(0).GetComponent<Slider>().value = (timeRemaining * 100) / totalTime;
        Timer.transform.GetChild(1).GetComponent<Slider>().value = (timeRemaining * 100) / totalTime;
        Timer.transform.GetChild(2).GetComponent<Text>().text = (int)minutes + ":" + decimals + (int)seconds;
    }
}
