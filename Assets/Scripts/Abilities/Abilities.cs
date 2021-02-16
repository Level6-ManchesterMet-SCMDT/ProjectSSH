using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Abilities : MonoBehaviourPunCallbacks
{

    [Header("General")]
    [SerializeField] GameObject Gun;
    [SerializeField] Camera Camera;

    [Header("Gunpowder Ability")]
    [SerializeField] GameObject GunpowderAbility;
    public int GunpowderAbilitypower = 0;
    public bool GunpowderAbilityUpgraded = false;

    //[Header("Pinpoint Smell Ability")]


    void Update()
    {
        if (this.photonView.IsMine && GunpowderAbilityUpgraded)
        {
            Camera.GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("Ground") | (1 << LayerMask.NameToLayer("GunpowderEffect")) | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Default")));
        }
    }

    //Pinpoint Smell Ability

    public void PinpointAbilityBought()
    {
        GunpowderAbilityUpgraded = true;
        GunpowderAbilitypower++;
    }

    //Gunpowder Ability

    public void ShootEffect()
    {
        GameObject GunpowderEffect = PhotonNetwork.Instantiate(GunpowderAbility.name, Gun.transform.position, this.transform.rotation, 0);
        StartCoroutine(respawnWait(GunpowderEffect, this.transform.GetComponent<Abilities>().GunpowderAbilitypower));
        GunpowderEffect.SetActive(false);
    }

    public void GupowderAbilityBought()
    {
        GunpowderAbilityUpgraded = true;
        GunpowderAbilitypower++;
    }

    IEnumerator respawnWait(GameObject GunpowderEffect, int GunpowderAbilitypower)
    {
        //Wait for 4 seconds
        yield return new WaitForSeconds(5 * GunpowderAbilitypower);
        PhotonNetwork.Destroy(GunpowderEffect);
    }
}
