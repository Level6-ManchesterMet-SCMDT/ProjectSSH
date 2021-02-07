using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerHit : MonoBehaviour
{
    [SerializeField] GameObject Player;

    public void TakeDamage(float damage, string deathAnim, string playerName)
    {
        Player.GetComponent<PlayerStats>().TakeDamage(damage, deathAnim, playerName);
    }
}