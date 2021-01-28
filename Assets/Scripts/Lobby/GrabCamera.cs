using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCamera : MonoBehaviour
{
    private bool hasCamera = false;
    private Vector3 cameraPosition;
    private Quaternion cameraRotation;
    private Transform theCamera = null;
    private GameObject thePlayer = null;

    public float blendFac = 0.05f;

    void Start()
    {
        // find a camera on this object or its children
        // save the camera position and rotation, and a reference to the camera object
        Camera camComponent = GetComponentInChildren<Camera>();
        camComponent.enabled = false;
        theCamera = camComponent.transform;
        cameraPosition = theCamera.position;
        cameraRotation = theCamera.rotation;
    }

    IEnumerator ReleaseOverTime()
    {
        hasCamera = false;
        Camera playerCamComponent = thePlayer.GetComponentInChildren<Camera>();
        Transform playerCamera = playerCamComponent.transform;

        Vector3 targetPos = playerCamera.position;
        Quaternion targetRot = playerCamera.rotation;

        bool done = false;
        while (!done)
        {
            Vector3 curPos = theCamera.position;
            Quaternion curRot = theCamera.rotation;
            // blend towards the target
            curPos = Vector3.Lerp(curPos, targetPos, blendFac);
            curRot = Quaternion.Slerp(curRot, targetRot, blendFac);
            theCamera.position = curPos;
            theCamera.rotation = curRot;
            done = (targetPos - curPos).sqrMagnitude < 0.001f && Quaternion.Dot(targetRot, curRot) > 0.999f;
            yield return null;
        }
        // give the player back control and disable this camera
        // enable the camera in this object
        theCamera.GetComponent<Camera>().enabled = false;
        thePlayer.SetActive(true);
    }

    public void ReleaseCamera()
    {
        // hand the camera back to the player
        StartCoroutine(ReleaseOverTime());
    }

    public void DisableCam()
    {
        //hasCamera = false;

        //Camera playerCamComponent = thePlayer.GetComponentInChildren<Camera>();
        //Transform playerCamera = playerCamComponent.transform;

        //theCamera = playerCamComponent.transform;

        //Camera cam = GetComponent<Camera>();
        //cam.enabled = false;
        
        //thePlayer.SetActive(true);
        ////theCamera.GetComponentInChildren<Camera>().enabled = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (hasCamera)
        {
            // get the current rotation and position of the camera
            Vector3 curPos = theCamera.position;
            Quaternion curRot = theCamera.rotation;
            // blend towards the target
            curPos = Vector3.Lerp(curPos, cameraPosition, blendFac);
            curRot = Quaternion.Slerp(curRot, cameraRotation, blendFac);
            theCamera.position = curPos;
            theCamera.rotation = curRot;
        }
    }

    public void GrabCameraFrom(GameObject player)
    {
        // switch off the player controller so it doesn't get input and the camera is disabled
        thePlayer = player;
        player.SetActive(false);
        // enable the camera in this object
        theCamera.GetComponent<Camera>().enabled = true;
        // copy the camera position and rotation from the player
        Camera playerCamComponent = player.GetComponentInChildren<Camera>();
        Transform playerCamera = playerCamComponent.transform;
        theCamera.position = playerCamera.position;
        theCamera.rotation = playerCamera.rotation;

        hasCamera = true;

    }
}
