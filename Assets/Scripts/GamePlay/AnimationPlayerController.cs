using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayerController : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w") && Input.GetKey("left shift"))
        {
            Debug.Log("Running");
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", true);
        }

        if (Input.GetKey("w") && !Input.GetKey("left shift"))
        {
            Debug.Log("Walking");
            animator.SetBool("IsWalking", true);
            animator.SetBool("IsRunning", false);
        }
        if (Input.GetKeyUp("w"))
        {
            Debug.Log("Stopped Walking");
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
        }
    }
}
