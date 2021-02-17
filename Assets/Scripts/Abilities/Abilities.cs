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

    [Header("Hound Ability")]
    [SerializeField] GameObject smellTrailEffect;
    public int houndAbilitypower = 0;
    public bool spawnedSmellTrail = false;
    GameObject playersSmellTrail;

    [Header("Outline Ability")]
    public int OutlineAbilitypower = 0;

    [Header("AimBot Ability")]
    public bool hasAimBot = false;
    public bool aimBotActive = false;

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

        if (Input.GetKeyDown("p") && hasAimBot)
        {
            ActivateAimBot();
        }
        if (aimBotActive)
        {
            AimBot();
        }
    }

    //Pinpoint Smell Ability

    public void PinpointAbilityBought()
    {
        if (points > 0 && !PinpointSmellAbilityUpgraded && smellPoints >= 3)
        {
            if(!PinpointSmellAbilityUpgraded)
            {
                AbilityCamera.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("PinpointEffect");
            }
            PinpointSmellAbilityUpgraded = true;
            PinpointSmellAbilitypower++;
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
            if(!GunpowderAbilityUpgraded)
            {
                Camera.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("GunpowderEffect");
            }
            GunpowderAbilityUpgraded = true;
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


    //Hound ability

    public void houndAbility()
    {
        if (points > 0)
        {
            Abilities[] houndS = FindObjectsOfType<Abilities>();

            houndAbilitypower++;
            points--;

            foreach (Abilities p in houndS)
            {
                p.SpawnSmellTrail(houndAbilitypower);
            }
        }
    }

    public void SpawnSmellTrail(int dist)
    {
        if (!spawnedSmellTrail)
        {
            if (!this.photonView.IsMine)
            {
                playersSmellTrail = Instantiate(smellTrailEffect);
                playersSmellTrail.transform.SetParent(this.transform);
                playersSmellTrail.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                spawnedSmellTrail = true;
            }
        }
        else
        {
            var main = playersSmellTrail.GetComponent<ParticleSystem>().main;
            main.startLifetime = dist;
        }
    }


    //outline ability

    public void OutlineAbility()
    {
        if (points > 0)
        {
            points--;
            OutlineAbilitypower++;
            sightPoints++;

            Outline[] outline = FindObjectsOfType<Outline>();

            switch (OutlineAbilitypower)
            {
                case 1:
                    Abilities[] players = FindObjectsOfType<Abilities>();


                    foreach (Abilities p in players)   // this will need to be changed foreach ability scrip not player manager script
                    {
                        p.AddOutlineAbility();   //changed to abilities
                    }
                    break;
                case 2:
                    foreach (Outline p in outline)
                    {
                        p.OutlineColor = Color.grey;
                    }
                    break;
                case 3:
                    foreach (Outline p in outline)
                    {
                        p.OutlineColor = Color.white;
                    }
                    break;
            }
        }
        
    }

    public void AddOutlineAbility()
    {
        if (!this.photonView.IsMine)
        {
            var Outline = gameObject.AddComponent<Outline>();
            Outline.OutlineMode = Outline.Mode.OutlineVisible;
            Outline.OutlineColor = Color.black;
            Outline.OutlineWidth = 3.0f;
        }
    }


    //aimbot ability 

    public void hasAimBotFunc()
    {
        if(points > 0 && !hasAimBot && sightPoints >= 3)
        {
            hasAimBot = true;
            points--;
        }
    }

    public void ActivateAimBot()
    {
        aimBotActive = true;
        this.gameObject.GetComponent<PlayerManager>().shopActive = true;
        StartCoroutine(DeActivateAimbot());
    }

    IEnumerator DeActivateAimbot()
    {
        yield return new WaitForSeconds(5);
        aimBotActive = false;
        this.gameObject.GetComponent<PlayerManager>().shopActive = false;
    }

    public void AimBot()
    {
        PlayerManager[] players = FindObjectsOfType<PlayerManager>();

        foreach (PlayerManager p in players)
        {
            if (p.GetComponent<Renderer>().isVisible)
            {
                if (!p.photonView.IsMine)
                {
                    Camera.transform.LookAt(p.gameObject.transform.position + new Vector3(0.0f, 1.4f, 0.0f));
                    return;
                }
            }
        }
    }
}
