using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] GameObject Player;

    public void TakeDamage(float damage, string deathAnim)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("TookDamage", RpcTarget.All, damage, deathAnim);
    }

    [PunRPC]
    void TookDamage(float damage, string deathAnim)
    {
        Player.GetComponent<PlayerStats>().TakeDamage(damage, deathAnim);
    }

}