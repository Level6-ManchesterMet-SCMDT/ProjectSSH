using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Abilities : MonoBehaviourPunCallbacks
{

    [Header("General")]
    [SerializeField] GameObject Gun;
    [SerializeField] Camera Camera;
    [SerializeField] Camera AbilityCamera;

    [Header("Gunpowder Ability")]
    [SerializeField] GameObject GunpowderAbility;
    public int GunpowderAbilitypower = 0;
    public bool GunpowderAbilityUpgraded = false;

    [Header("Pinpoint Smell Ability")]
    [SerializeField] GameObject PinpointSmellAbility;
    public int PinpointSmellAbilitypower = 0;
    public bool PinpointSmellAbilityUpgraded = false;

    void Update()
    {
    }

    //Pinpoint Smell Ability

    public void PinpointAbilityBought()
    {
        PinpointSmellAbilityUpgraded = true;
        AbilityCamera.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("PinpointEffect");
        PinpointSmellAbilitypower++;
        SmellEffect();
    }

    public void SmellEffect()
    {
        Abilities[] Players = FindObjectsOfType<Abilities>();
        foreach (Abilities player in Players)
        {
            if (player.GetComponent<PhotonView>().ViewID != this.GetComponent<PhotonView>().ViewID)
            {
                GameObject SmellEffect = PhotonNetwork.Instantiate(PinpointSmellAbility.name, player.transform.position, PinpointSmellAbility.transform.rotation, 0);
                SmellEffect.transform.parent = player.transform;
                DontDestroyOnLoad(SmellEffect.gameObject);
                StartCoroutine(despawnWaitPinpoint(SmellEffect, PinpointSmellAbilitypower));
            }
        }
    }


    IEnumerator despawnWaitPinpoint(GameObject SmellEffect, int PinpointSmellAbilitypower)
    {
        yield return new WaitForSeconds(5 * PinpointSmellAbilitypower);
        PhotonNetwork.Destroy(SmellEffect);
    }

    //Gunpowder Ability

    public void ShootEffect()
    {
        GameObject GunpowderEffect = PhotonNetwork.Instantiate(GunpowderAbility.name, Gun.transform.position, this.transform.rotation, 0);
        StartCoroutine(despawnWaitGunpowder(GunpowderEffect, this.transform.GetComponent<Abilities>().GunpowderAbilitypower));
        GunpowderEffect.SetActive(false);
    }

    public void GupowderAbilityBought()
    {
        GunpowderAbilityUpgraded = true;
        Camera.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("GunpowderEffect");
        GunpowderAbilitypower++;
    }

    IEnumerator despawnWaitGunpowder(GameObject GunpowderEffect, int GunpowderAbilitypower)
    {
        yield return new WaitForSeconds(5 * GunpowderAbilitypower);
        PhotonNetwork.Destroy(GunpowderEffect);
    }
}
