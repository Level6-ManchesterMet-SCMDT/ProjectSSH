using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Abilities : MonoBehaviourPunCallbacks
{
    [Header("General")]

    [SerializeField] GameObject Gun;
    [SerializeField] GameObject Camera;

    //Gunpowder Smell
    [Header("Gunpowder Smell")]

    [SerializeField] GameObject GunpowderAbilityEffect;

    public int GunpowderAbilitypower = 1;
    public bool GunpowderAbilityUpgraded = false;

    //Gunpowder Smell

    void Update()
    {
        if (GunpowderAbilityUpgraded)
        {
            Debug.Log("Test");
            //Camera.GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("Ground") | (1 << LayerMask.NameToLayer("GunpowderEffect")) | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Default")));
            Camera.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("GunpowderEffect");
        }
    }

    public void GunpowderUpgraded()
    {
        this.photonView.RPC("GunpowderUpgraded_RPC", RpcTarget.All);
    }

    public void ShootEffect()
    {
        GameObject GunpowderEffect = PhotonNetwork.Instantiate(GunpowderAbilityEffect.name, Gun.transform.position, this.transform.rotation, 0);
        StartCoroutine(respawnWait(GunpowderEffect, this.transform.GetComponent<Abilities>().GunpowderAbilitypower));
        GunpowderEffect.SetActive(false);
    }

    IEnumerator respawnWait(GameObject GunpowderEffect, int GunpowderAbilitypower)
    {
        //Wait for 4 seconds
        yield return new WaitForSeconds(5 * GunpowderAbilitypower);
        PhotonNetwork.Destroy(GunpowderEffect);
    }

    [PunRPC]
    public void GunpowderUpgraded_RPC()
    {
        GunpowderAbilitypower++;
        GunpowderAbilityUpgraded = true;
    }
}
