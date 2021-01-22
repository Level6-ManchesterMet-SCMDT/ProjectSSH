using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListing : MonoBehaviourPunCallbacks
{

    public GameObject roomCard;
    public GameObject roomListing;

    private Dictionary<string, GameObject> roomListingCards;

    private void Start()
    {
        roomListingCards = new Dictionary<string, GameObject>();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList == false)
            {
                if (!roomListingCards.ContainsKey(room.Name))
                {

                    GameObject card = Instantiate(roomCard);
                    card.transform.SetParent(roomListing.transform);
                    card.transform.localScale = Vector3.one;
                    roomListingCards.Add(room.Name, card);
                }
                roomListingCards[room.Name].GetComponent<RoomCard>().setValues(room.Name, room.PlayerCount);
            }
            else
            {
                roomListingCards.Remove(room.Name);
            }
        }
    }
}
