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
}
