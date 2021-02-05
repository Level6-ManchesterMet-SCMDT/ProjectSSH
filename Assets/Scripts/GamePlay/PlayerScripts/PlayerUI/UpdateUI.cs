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

    private float timeRemaining;
    private float totalTime;
    private float seconds = 0;
    private float minutes;
    private string decimals = "0";
    private bool hasUpdated = false;
    private string currentGameMode;

    void Update()
    {
        if (!hasUpdated)
        {
            return;
        }

        switch(currentGameMode)
        {
            case "TeamDeathMatch":
                MatchTimer();
                break;
            default:
                break;
        }

    }

    public void UpdateValues(float maxTime, string gameMode)
    {
        timeRemaining = maxTime;
        totalTime = maxTime;
        currentGameMode = gameMode;
        hasUpdated = true;
    }

    public void RoundOver()
    {
        this.transform.GetChild(4).gameObject.SetActive(true);
    }

    public void UpdateScores(List<string> playerNames, List<string> playerKills, List<string> playerDeaths, string MapName)
    {
        for(int i = 0; i < playerNames.Count; i++)
        {
            Score[i].SetActive(true);
            Score[i].transform.GetChild(0).GetComponent<Text>().text = playerNames[i];
            Score[i].transform.GetChild(1).GetComponent<Text>().text = playerKills[i];
            Score[i].transform.GetChild(2).GetComponent<Text>().text = playerDeaths[i];
        }
    }

    void MatchTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }

        Timer.transform.GetChild(0).GetComponent<Slider>().value = (timeRemaining * 100) / totalTime;
        Timer.transform.GetChild(1).GetComponent<Slider>().value = (timeRemaining * 100) / totalTime;

        if (timeRemaining > 59)
        {
            minutes = timeRemaining / 60;
            if (seconds <= 0)
            {
                seconds = 60;
            }
        }
        else {
            seconds = timeRemaining;
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

        Timer.transform.GetChild(2).GetComponent<Text>().text = (int)minutes + ":" + decimals + (int)seconds;
    }
}
