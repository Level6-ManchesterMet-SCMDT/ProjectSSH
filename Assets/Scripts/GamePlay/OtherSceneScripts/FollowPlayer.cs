using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    Abilities playerToFollow = null;

    public void FollowPlayers(Abilities player)
    {
        playerToFollow = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerToFollow)
        {
            this.transform.position = playerToFollow.transform.position;
        }
        
    }
}
