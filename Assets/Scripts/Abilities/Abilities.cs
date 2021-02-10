using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Abilities : MonoBehaviourPunCallbacks
{

    [SerializeField] GameObject GunpowderAbility;
    [SerializeField] GameObject Gun;

    public bool GunpowderAbilityPurchased = false;
    public int GunpowderAbilitypower = 1;

    public void ShootEffect()
    {
        this.photonView.RPC("RPC_ShootEffect", RpcTarget.Others);
    }

    // Update is called once per frame
    void Update()
    {
        if (GunpowderAbilityPurchased)
        {
            Gun.GetComponent<Camera>().cullingMask = (1 << LayerMask.NameToLayer("FPS") | (1 << LayerMask.NameToLayer("GunpowderEffect")));
        }
    }

    [PunRPC]
    public void RPC_ShootEffect()
    {
        GameObject GunpowderEffect = PhotonNetwork.Instantiate(GunpowderAbility.name, Gun.transform.position, this.transform.rotation, 0);
        StartCoroutine(respawnWait(GunpowderEffect, this.transform.GetComponent<Abilities>().GunpowderAbilitypower));
    }

    IEnumerator respawnWait(GameObject GunpowderEffect, int GunpowderAbilitypower)
    {
        //Wait for 4 seconds
        yield return new WaitForSeconds(5 * GunpowderAbilitypower);
        Destroy(GunpowderEffect);

    }
}
