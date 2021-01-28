using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


[RequireComponent(typeof(InputField))]

public class InputBox : MonoBehaviourPunCallbacks
{
    private const string playerNamePref = "PlayerName";

    void Start()
    {
        InputField inputField = GetComponent<InputField>();
        string startName = "";
        if(inputField != null)
        {
            if(PlayerPrefs.HasKey(playerNamePref))
            {
                startName = PlayerPrefs.GetString(playerNamePref);
                inputField.text = startName;
            }
        }
        PhotonNetwork.NickName = startName;
    }

    public void SetNickname(string name)
    {
        if (name == null)
        {
            PhotonNetwork.NickName = "player" + (PhotonNetwork.LocalPlayer.ActorNumber).ToString();
            return;
        }
        else
        {
            PhotonNetwork.NickName = name;
            PlayerPrefs.SetString(playerNamePref, name);
        }
    }
}