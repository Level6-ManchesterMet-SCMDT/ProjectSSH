using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HoundSmell : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject smellTrail;

    GameObject playersSmellTrail;
    bool spawnedSmellTrail;



    public void houndAbility()
    {
        HoundSmell[] houndS = FindObjectsOfType<HoundSmell>();

        foreach (HoundSmell p in houndS)
        {
            p.SpawnSmellTrail(1);
            p.SpawnSmellTrail(3);
        }
    }

    public void SpawnSmellTrail(int dist)
    {
        if (!spawnedSmellTrail)
        {
            if (!this.photonView.IsMine)
            {
                playersSmellTrail = Instantiate(smellTrail);
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

    public void OutlineAbility(int num)
    {
        Outline[] outline = FindObjectsOfType<Outline>();

        switch (num)
        {
            case 1:
                PlayerManager[] players = FindObjectsOfType<PlayerManager>();


                foreach (PlayerManager p in players)   // this will need to be changed foreach ability scrip not player manager script
                {
                    //p.AddOutlineAbility();   //changed to abilities
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

    public void AddOutlineAbility()
    {
        if (!this.photonView.IsMine)
        {
            var Outline = gameObject.AddComponent<Outline>();
            Outline.OutlineColor = Color.black;
            Outline.OutlineWidth = 3.0f;
        }
    }
}
