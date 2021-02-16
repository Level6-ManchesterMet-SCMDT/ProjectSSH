using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1";

    public GameObject connectPanel;
    public GameObject homePanel;
    public GameObject lobbyPanel;
    public GameObject PlayerListing;

    public GameObject Model;
    public GameObject[] pModelTransforms;
    GameObject pModel;

    public GameObject[] grabCamTransforms;
    GrabCamera grabCam;

    public Camera mainCam;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        int randomModel = Random.Range(0, pModelTransforms.Length);
        PlayerListing.GetComponent<PlayerListing>().currentPlayerPose = randomModel;
        //int randomModel = Random.Range(3,3);
        grabCam = FindObjectOfType<GrabCamera>();
        grabCam.transform.position = grabCamTransforms[randomModel].transform.position;
        grabCam.transform.rotation = grabCamTransforms[randomModel].transform.rotation;

        pModel = Instantiate(Model, pModelTransforms[randomModel].transform.position, Quaternion.identity);
        pModel.transform.rotation = pModelTransforms[randomModel].transform.rotation;

        pModel.GetComponent<Animator>().SetInteger("RandomAnimation", randomModel);

    }


    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by pun");
        PhotonNetwork.JoinLobby();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDisconnected() was called by PUN with reason {0}", cause);

    }
    public override void OnJoinedLobby()
    {
        grabCam.GrabCameraFrom(mainCam.gameObject);
        Debug.Log("Connected");
        connectPanel.SetActive(false);
        homePanel.SetActive(true);

    }
    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom() was called by pun. This client has joined a room");
        lobbySwitch();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("hello");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called by pun. No random room avaliable");
    }


    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }
    public void leaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Lobby");
    }

    public void lobbySwitch()
    {
        homePanel.SetActive(false);
        lobbyPanel.SetActive(true);
        pModel.gameObject.SetActive(false);
        //grabCam.DisableCam();
        //mainCam.enabled = false;
    }

}
