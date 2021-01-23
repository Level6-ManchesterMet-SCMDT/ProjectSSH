using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class PlayerCard : MonoBehaviourPunCallbacks
{


    public Text pName;
    public Text idNum;
    public Text pReady;

    public GameObject readyBut;

    private ExitGames.Client.Photon.Hashtable Cproperties = new ExitGames.Client.Photon.Hashtable();


    // Start is called before the first frame update


    public void setValue(string name, string idNumber)
    {
        pName.text = name;
        idNum.text = idNumber;


        if (name == PhotonNetwork.LocalPlayer.NickName)
        {
            readyBut.SetActive(true);
            bool notReady = false;
            Cproperties["Ready"] = notReady;
            PhotonNetwork.SetPlayerCustomProperties(Cproperties);
        }
    }

    public void readyUp()
    {
        bool ready = true;
        Cproperties["Ready"] = ready;
        PhotonNetwork.SetPlayerCustomProperties(Cproperties);
        readyBut.SetActive(false);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (pName.text == targetPlayer.NickName)
        {
            if((bool)changedProps["Ready"] == true)
            {
                pName.color = Color.green;
            }
        }
    }
}
