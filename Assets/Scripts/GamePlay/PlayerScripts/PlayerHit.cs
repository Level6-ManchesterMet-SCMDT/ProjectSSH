using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHit : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Player;

    public void TakeDamage(float damage, string deathAnim, string playerName)
    {
        this.photonView.RPC("RPC_TakeDamage", RpcTarget.All, damage, deathAnim, playerName);
    }

    [PunRPC]

    public void RPC_TakeDamage(float damage, string deathAnim, string playerName)
    {
        Player.GetComponent<PlayerStats>().TakeDamage(damage, deathAnim, playerName);
    }
}