using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UpdateUI : MonoBehaviourPunCallbacks
{
    public GameObject Timer;

    private float timeRemaining;
    private float totalTime;
    private float seconds;
    private float minutes;
    private bool hasUpdated = false;
    private string currentGameMode;


    void Update()
    {
        Debug.Log("Update " + hasUpdated);
        Debug.Log("PhotonNetworkUpdate " + PhotonNetwork.LocalPlayer);

        if (hasUpdated)
        {
            switch (currentGameMode)
            {
                case "TeamDeathMatch":
                    DeathMatch();
                    break;
                default:
                    break;
            }
        }
    }

    public void UpdateValues(float maxTime, string gameMode)
    {
        timeRemaining = maxTime;
        totalTime = maxTime;
        currentGameMode = gameMode;
        hasUpdated = true;
        Debug.Log("UpdateValues " + hasUpdated);
        Debug.Log("PhotonNetwork " + PhotonNetwork.LocalPlayer);
    }

    void DeathMatch()
    {
        Debug.Log("DeathMatch");
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }

        Timer.transform.GetChild(0).GetComponent<Slider>().value = (timeRemaining * totalTime) / 100;
        Timer.transform.GetChild(1).GetComponent<Slider>().value = (timeRemaining * totalTime) / 100;

        seconds = timeRemaining;

        if (seconds > 59)
        {
            minutes = seconds / 60;
            minutes = (int)minutes;
        }
        else minutes = 0;

        Timer.transform.GetChild(2).GetComponent<Text>().text = minutes + ":" + seconds;
    }
}
