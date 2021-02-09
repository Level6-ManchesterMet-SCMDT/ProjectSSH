using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UpdateUI : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Player;
    [SerializeField] Camera Camera;
    [SerializeField] Camera FPSCamera;

    public GameObject Timer;
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

    public void RoundOver()
    {
        Debug.Log("Test2");
        this.transform.GetChild(0).gameObject.SetActive(false);
        this.transform.GetChild(1).gameObject.SetActive(false);
        this.transform.GetChild(2).gameObject.SetActive(false);
        this.transform.GetChild(3).gameObject.SetActive(false);
        this.transform.GetChild(4).gameObject.SetActive(true);
        //this.transform.GetChild(5).gameObject.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Player.GetComponent<PlayerManager>().keyboardEnabled = false;
        Camera.enabled = false;
        FPSCamera.enabled = false;
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

    public void BackToLobby()
    {
        PhotonNetwork.LoadLevel("DemoTesting");
    }
}
