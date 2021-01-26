using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationPlayerController : MonoBehaviourPunCallbacks
{
    Animator _animator;
    PlayerController _playerController;

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
            var leftRightPressed  = Input.GetAxis("Horizontal");
            var forwardBackwardPressed = Input.GetAxis("Vertical");

            if(!Input.GetKey("left shift"))
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
        _animator.SetFloat("VelX", x);
        _animator.SetFloat("VelY", y);
    }
}
