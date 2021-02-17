using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Abilities : MonoBehaviourPunCallbacks
{

    [Header("General")]
    [SerializeField] GameObject Gun;
    [SerializeField] Camera Camera;
    [SerializeField] Camera AbilityCamera;
    [SerializeField] Text pointsText;
    public int points = 0;
    public bool rareCooldown = true;
    public int sightPoints = 0;
    public int smellPoints = 0;

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
        pointsText.text = points.ToString();
        if ( Input.GetKeyDown("q")  && rareCooldown)
        {
            if (PinpointSmellAbilityUpgraded)
            {
                
                SmellEffect();
                rareCooldown = false;
                Debug.Log("AbilityStarted");
;
            }
            //else if (AimbotAbilityUpgraded)
            //{

            //}
        }
    }

    //Pinpoint Smell Ability

    public void PinpointAbilityBought()
    {
        if (points > 0 && !PinpointSmellAbilityUpgraded && smellPoints >= 3)
        {
            PinpointSmellAbilityUpgraded = true;
            PinpointSmellAbilitypower++;
            AbilityCamera.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("PinpointEffect");
            points--;
        }
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
        StartCoroutine(pinpointCooldown());

    }

    IEnumerator pinpointCooldown()
    {
        yield return new WaitForSeconds(10);
        rareCooldown = true;
        Debug.Log("CooldownEnded" + rareCooldown);

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
        if (points > 0)
        {
            GunpowderAbilityUpgraded = true;
            Camera.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("GunpowderEffect");
            GunpowderAbilitypower++;
            points--;
            smellPoints++;
        }
    }

    IEnumerator despawnWaitGunpowder(GameObject GunpowderEffect, int GunpowderAbilitypower)
    {
        yield return new WaitForSeconds(5 * GunpowderAbilitypower);
        PhotonNetwork.Destroy(GunpowderEffect);
    }

    //Points Management

    public void AddPoints(string playerKiller)
    {
        if (PhotonNetwork.LocalPlayer.NickName == playerKiller)
        {
            points++;
        }
    }

}
