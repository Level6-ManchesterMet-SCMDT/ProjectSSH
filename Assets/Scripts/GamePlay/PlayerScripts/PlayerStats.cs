using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerStats : MonoBehaviourPunCallbacks
{
    
    [SerializeField] GameObject healthBarRef;
    [SerializeField] GameObject rog_layers_hand_IK;
    [SerializeField] GameObject UpdateUI;
    [SerializeField] GameObject playerHead;
    [SerializeField] Camera FPSCam;

    public float maxHealth = 100f;
    public Animator animator;
    public Scoreboard_Updater sbUpdater;
    public float currentHealth;
    public bool dead = false;

    private HealthBar healthBar;

    TwoBoneIKConstraint constraintLeftHand;

    private void Start()
    {
        Spawned();

    }

    public void Spawned()
    {
        FPSCam.enabled = true;
        currentHealth = maxHealth;
        healthBar = healthBarRef.GetComponent<HealthBar>();
        healthBar.SetMaxHealth(maxHealth);
        constraintLeftHand = rog_layers_hand_IK.transform.GetChild(1).GetComponent<TwoBoneIKConstraint>();
        sbUpdater = FindObjectOfType<Scoreboard_Updater>();
        UpdateUI.SetActive(true);
        this.GetComponent<PlayerManager>().keyboardEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(20f, "Death", "");
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealDamage(20f);
        }

        if (dead)
        {
            constraintLeftHand.data.targetPositionWeight -= 0.01f;
        }
    }

    public void TakeDamage(float damage, string deathAnim, string playerReference)
    {
        currentHealth -= damage;

        if(currentHealth < 0f)
        {
            currentHealth = 0f;
        }

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0f && dead == false)
        {
            dead = true;
            FPSCam.enabled = false;
            PhotonView photonView = PhotonView.Get(this);
            sbUpdater.enemyKilled(photonView.Owner.NickName, playerReference);
            this.GetComponent<PlayerManager>().RespawnPlayer(deathAnim);
            animator.SetBool(deathAnim, true);
            UpdateUI.SetActive(false);
            this.GetComponent<PlayerManager>().keyboardEnabled = false;
        }
    }

    void RPC_PlayerDied(string playerDied, string playerKiller)
    {
        UpdateUI.GetComponent<UpdateUI>().PlayerDied(playerDied, playerKiller);
    }

    public void HealDamage(float damage)
    {
        currentHealth += damage;

        if (currentHealth > 100f)
        {
            currentHealth = 100f;
        }

        healthBar.SetHealth(currentHealth);
    }
}
