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
    [SerializeField] GameObject CommonAbilities;
    [SerializeField] Button SightRare;
    [SerializeField] Button SmellRare;
    [SerializeField] Image RareAbilityImage;
    [SerializeField] Image RareAbilityImageBackground;

    public int points = 0;
    public bool rareCooldown = true;
    public int sightPoints = 0;
    public int smellPoints = 0;
    Button[] allChildren;

    [Header("Gunpowder Ability")]
    [SerializeField] GameObject GunpowderAbility;
    [SerializeField] Text GunpowderUpgradeNumberText;
    public int GunpowderAbilitypower = 0;
    public bool GunpowderAbilityUpgraded = false;

    [Header("Pinpoint Smell Ability")]
    [SerializeField] GameObject PinpointSmellAbility;
    public int PinpointSmellAbilitypower = 0;
    public bool PinpointSmellAbilityUpgraded = false;
    public Sprite SmellSprite;

    [Header("Hound Ability")]
    [SerializeField] GameObject smellTrailEffect;
    [SerializeField] Text HoundUpgradeNumberText;
    public int houndAbilitypower = 0;
    public bool spawnedSmellTrail = false;
    GameObject playersSmellTrail;

    [Header("Outline Ability")]
    [SerializeField] Text OutlineUpgradeNumberText;
    public int OutlineAbilitypower = 0;

    [Header("Zoom Ability")]
    [SerializeField] Text ZoomUpgradeNumberText;
    public int ZoomAbilitypower = 0;
    public bool ZoomAbilityUpgraded = false;

    [Header("AimBot Ability")]
    [SerializeField] PlayerManager PM;
    public bool hasAimBot = false;
    public bool aimBotActive = false;
    public Sprite AimBotSprite;

    void Awake()
    {
        allChildren = CommonAbilities.GetComponentsInChildren<Button>();
    }

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
            }
        }

        if (Input.GetKeyDown("q") && hasAimBot)
        {
            ActivateAimBot();
        }
        if (aimBotActive)
        {
            AimBot();
        }

        if(points <= 0)
        {
            foreach(Button child in allChildren)
            {
                child.interactable = false;
            }
        }
        else
        {
            foreach (Button child in allChildren)
            {
                child.interactable = true;
            }
        }

        if (sightPoints >= 3 && !PinpointSmellAbilityUpgraded && points >= 1 && !hasAimBot)
        {
            SightRare.interactable = true;
        }
        else
        {
            SightRare.interactable = false;
        }
            
        if (smellPoints >= 3 && !hasAimBot && points >= 1 && !PinpointSmellAbilityUpgraded )
        {
            SmellRare.interactable = true;
        }
        else
        {
            SmellRare.interactable = false;
        }
    }

    //Pinpoint Smell Ability

    public void PinpointAbilityBought()
    {
        if (!PinpointSmellAbilityUpgraded && smellPoints >= 3)
        {
            if(!PinpointSmellAbilityUpgraded)
            {
                AbilityCamera.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("PinpointEffect");
            }
            PinpointSmellAbilityUpgraded = true;
            PinpointSmellAbilitypower++;
            points--;
            RareAbilityImageBackground.GetComponent<Image>().color = Color.green;
            RareAbilityImage.GetComponent<Image>().sprite = SmellSprite;
        }
    }

    public void SmellEffect()
    {
        Debug.Log("SmellEffect");
        Abilities[] Players = FindObjectsOfType<Abilities>();
        foreach (Abilities player in Players)
        {
            if (player.GetComponent<PhotonView>().ViewID != this.GetComponent<PhotonView>().ViewID)
            {
                Debug.Log("If");
                GameObject SmellEffect = PhotonNetwork.Instantiate(PinpointSmellAbility.name, player.transform.position, PinpointSmellAbility.transform.rotation, 0);
                SmellEffect.GetComponent<FollowPlayer>().FollowPlayers(player);
                DontDestroyOnLoad(SmellEffect.gameObject);
                StartCoroutine(despawnWaitPinpoint(SmellEffect, PinpointSmellAbilitypower));
                StartCoroutine(pinpointCooldown(PinpointSmellAbilitypower));
            }
            else
            {
                StartCoroutine(pinpointCooldown(PinpointSmellAbilitypower));
            }
        }
        
    }


    IEnumerator despawnWaitPinpoint(GameObject SmellEffect, int PinpointSmellAbilitypower)
    {
        yield return new WaitForSeconds(5 * PinpointSmellAbilitypower);
        PhotonNetwork.Destroy(SmellEffect);
    }

    IEnumerator pinpointCooldown(int PinpointSmellAbilitypower)
    {
        RareAbilityImageBackground.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(10 + PinpointSmellAbilitypower);
        RareAbilityImageBackground.GetComponent<Image>().color = Color.green;
        rareCooldown = true;
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
            GunpowderUpgradeNumberText.text = GunpowderAbilitypower.ToString();
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
            HoundUpgradeNumberText.text = houndAbilitypower.ToString();
            foreach (Abilities p in houndS)
            {
                p.SpawnSmellTrail(houndAbilitypower, p);
            }
        }
    }

    public void SpawnSmellTrail(int dist, Abilities player)
    {
        if (!spawnedSmellTrail)
        {
            if (!this.photonView.IsMine)
            {
                playersSmellTrail = Instantiate(smellTrailEffect);
                playersSmellTrail.GetComponent<FollowPlayer>().FollowPlayers(player);
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
            OutlineUpgradeNumberText.text = OutlineAbilitypower.ToString();
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

    //Zoom ability

    public void ZoomAbilityBought()
    {
        if (ZoomAbilitypower <= 3)
        {
            if (!ZoomAbilityUpgraded)
            {
                Camera.GetComponent<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("ZoomEffect");
            }
            ZoomAbilityUpgraded = true;
            ZoomAbilitypower++;
            PlayerManager.UpdateZoom(ZoomAbilitypower);
            points--;
            sightPoints++;
            ZoomUpgradeNumberText.text = ZoomAbilitypower.ToString();
        }
    }
    //aimbot ability 

    public void AimBotAbilityBought()
    {
        RareAbilityImageBackground.GetComponent<Image>().color = Color.green;
        RareAbilityImage.GetComponent<Image>().sprite = AimBotSprite;
        if (points > 0 && !hasAimBot && sightPoints >= 3)
        {
            hasAimBot = true;
            points--;
        }
    }

    public void ActivateAimBot()
    {
        aimBotActive = true;
        PM.shopActive = true;
        StartCoroutine(DeActivateAimbot());
    }

    IEnumerator DeActivateAimbot()
    {
        RareAbilityImageBackground.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(5);
        RareAbilityImageBackground.GetComponent<Image>().color = Color.green;
        aimBotActive = false;
        PM.shopActive = false;
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
