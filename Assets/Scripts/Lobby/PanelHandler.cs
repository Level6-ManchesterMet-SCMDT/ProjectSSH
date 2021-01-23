using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject roomListP;
    public GameObject createRoomP;


    public void onHome()
    {
        roomListP.SetActive(false);
        createRoomP.SetActive(false);
    }

    public void onRooms()
    {
        roomListP.SetActive(true);
        createRoomP.SetActive(false);
    }
    public void onCreate()
    {
        roomListP.SetActive(false);
        createRoomP.SetActive(true);
    }
}
