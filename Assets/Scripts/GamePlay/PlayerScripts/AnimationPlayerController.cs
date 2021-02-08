using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationPlayerController : MonoBehaviourPunCallbacks
{
    Animator _animator;

    public float MaxSpeed = 10;

    void Start()
    {
        if (photonView.IsMine)
        {
            _animator = GetComponent<Animator>();
        }
    }


    void Update()
    {
        if (photonView.IsMine)
        {
            var leftRightPressed  = Input.GetAxis("Horizontal"); //gets movement of mouse
            var forwardBackwardPressed = Input.GetAxis("Vertical");

            if(!Input.GetKey("left shift")) //if player isnt pressing shift set the leftrightpressed an forwardrightpressed to the horizontal and vertical values
            {
                if(forwardBackwardPressed > 0.5f)
                {
                    forwardBackwardPressed = 0.5f;
                }

                if (leftRightPressed > 0.5f)
                {
                    leftRightPressed = 0.5f;
                }

                if (leftRightPressed < -0.5f)
                {
                    leftRightPressed = -0.5f;
                }
            }

            Move(leftRightPressed, forwardBackwardPressed);
        }
    }

    private void Move(float x, float y)
    {
        _animator.SetFloat("VelX", x); //send those values to the animator so that it plays the correct animation
        _animator.SetFloat("VelY", y);
    }
}
