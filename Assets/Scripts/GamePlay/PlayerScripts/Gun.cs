using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;

public class Gun : MonoBehaviourPunCallbacks
{
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 30;
    public float fireRate = 15f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public ParticleSystem cartridgeEffect;
    public GameObject genericImpactEffect;
    public GameObject playerImpactEffect;

    private AmmoCount UIAmmo;
    private float nextTimeToFire = 0f;

    public GameObject rog_layers_hand_IK;
    public Animator animator;

    TwoBoneIKConstraint constraintLeftHand;

    void Start ()
    {
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            currentAmmo = maxAmmo;
            UIAmmo = GameObject.Find("Canvas/UI/MaxAmmo/CurrentAmmo").GetComponent<AmmoCount>();
            UIAmmo.ammo = currentAmmo;
            UIAmmo.maxAmmo = maxAmmo;
            constraintLeftHand = rog_layers_hand_IK.transform.GetChild(1).GetComponent<TwoBoneIKConstraint>();
        }
    }

    void Update()
    {

        if (isReloading)
        {
            constraintLeftHand.data.targetPositionWeight -= 0.01f;
            Debug.Log("Test");
        }
        else
            constraintLeftHand.data.targetPositionWeight += 0.01f;

        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            if (isReloading)
            {
                return;
            }

            if (currentAmmo <= 0)
            {
                StartCoroutine(Reload());
                return;
            }

            Shoot();
            
            UIAmmo.ammo = currentAmmo;
        }

    }

    IEnumerator Reload ()
    {
        isReloading = true;
        Debug.Log("Reloading...");
        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);

        animator.SetBool("Reloading", false);

        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;
        isReloading = false;
    }
    
    void Shoot()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            muzzleFlash.Play(); //play muzzleflash on shooting
            cartridgeEffect.Play();
            currentAmmo--;

            RaycastHit hit;
            if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                Target target = hit.transform.GetComponent<Target>();

                if(target != null)
                {
                    target.TakeDamage(damage); //deal damage to object or player
                }

                if(hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce); //add force to object with rigidbody
                }
                if (hit.transform.tag == "Player")
                {
                    GameObject impactGO = Instantiate(playerImpactEffect, hit.point, Quaternion.LookRotation(hit.normal)); //spawn impact effect on target
                    Destroy(impactGO, 2f);
                }
                else
                {
                    GameObject impactGO = Instantiate(genericImpactEffect, hit.point, Quaternion.LookRotation(hit.normal)); //spawn impact effect on target
                    Destroy(impactGO, 2f);
                }

            }
        }
    }
}
