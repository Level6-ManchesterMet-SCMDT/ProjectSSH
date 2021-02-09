using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHead : MonoBehaviour
{
    [SerializeField] GameObject playerHead;


    void Awake()
    {
    }

    void CameraDeath()
    {
        this.transform.position = new Vector3(playerHead.transform.position.x, playerHead.transform.position.y, playerHead.transform.position.z);
    }
}
