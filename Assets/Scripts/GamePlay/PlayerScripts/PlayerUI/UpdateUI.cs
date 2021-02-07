using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UpdateUI : MonoBehaviourPunCallbacks
{
    public GameObject Timer;
    public GameObject Player;
    public GameObject[] Score;

    private string currentGameMode;
    private List<string> playerNames;
    private List<int> playerKills;
    private List<int> playerDeaths;


    void Update()
    {
        switch(currentGameMode)
        {
            case "TeamDeathMatch":
                break;
            default:
                break;
        }
    }

    public void playerInstantiated(string[] intantiatedPlayerNames)
    {
        playerNames = new List<string>();
        playerKills = new List<int>();
        playerDeaths = new List<int>();

        foreach (var players in intantiatedPlayerNames)
        {
            playerNames.Add(players);
        }

        playerKills.Add(0);
        playerDeaths.Add(0);
        UpdateScores();
    }

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
            }

            if (playerKiller == playerNames[i])
            {
                playerKills[i]++;
            }
        }
        UpdateScores();
    }

    public void UpdateScores()
    {
        for(int i = 0; i < playerNames.Count; i++)
        {
            Score[i].SetActive(true);
            Score[i].transform.GetChild(0).GetComponent<Text>().text = playerNames[i];
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
