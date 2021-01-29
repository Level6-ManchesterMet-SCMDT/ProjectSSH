using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] GameObject cameraHolder;
    [SerializeField] Animator animator;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject right_hand_IK;
    [SerializeField] GameObject left_hand_IK;
    [SerializeField] GameObject rog_layers_hand_IK;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float mouseSensitivity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public static GameObject LocalPlayerInstance;

    private Vector3 moveDirection = Vector3.zero;
    private PhotonView PV;

    float verticalLookRotation;
    bool grounded;
    bool Armed = true;
    bool FinishedJumping = false;
    bool isGrounded;
    bool holdingGun = true;

    Vector3 velocity;

    Rig constrainthands;
    TwoBoneIKConstraint constraintRightHand;
    TwoBoneIKConstraint constraintLeftHand;
    RigBuilder rb;

    public CharacterController player;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);

        }
        else
        {

            constrainthands = rog_layers_hand_IK.GetComponent<Rig>();
            constraintRightHand = rog_layers_hand_IK.transform.GetChild(0).GetComponent<TwoBoneIKConstraint>();
            constraintLeftHand = rog_layers_hand_IK.transform.GetChild(1).GetComponent<TwoBoneIKConstraint>();
            rb = transform.GetComponent<RigBuilder>();
        }
    }

    void Update()
    {
        if(photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            Gravity();
            Look();
            Move();
            Jump();
            Rifle();
            if (holdingGun) {
                constrainthands.weight += 0.01f;
            }
            else
                constrainthands.weight -= 0.01f;
        }
    }

    void Rifle()
    {
        if (Input.GetKey("f") && !Armed)
        {
            animator.SetBool("Armed", true);

        }
        if (Input.GetKey("f") && Armed)
        {
            animator.SetBool("Armed", false);
        }

        if (Input.GetKey(KeyCode.Mouse1) && Armed)
        {
            animator.SetBool("Aiming", true);
        }
        else
        {
            animator.SetBool("Aiming", false);
        }
    }


    void Gravity()
    {
        velocity.y += gravity * Time.deltaTime;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        player.Move(move * speed * Time.deltaTime);
        player.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetBool("Jumped", true);
        }

        else if (FinishedJumping) 
        {
            animator.SetBool("Jumped", false);
        }
        FinishedJumping = false;
    }

    void FinishedJump()
    {
        FinishedJumping = true;
    }

    void FinishedPuttingBack()
    {
        Armed = false;
        gun.SetActive(false);
    }

    void StartEquipping()
    {
        gun.SetActive(true);
    }

    void FinishedEquipping()
    {
        Armed = true;
        SetHoldingGunState(true);
    }

    void StartedPuttingBack()
    {
        SetHoldingGunState(false);
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    [PunRPC]
    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    [PunRPC]
    public void SetHoldingGunState(bool _holdingGun)
    {
        holdingGun = _holdingGun;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(grounded);
            stream.SendNext(holdingGun);
        }
        else
        {
            this.grounded = (bool)stream.ReceiveNext();
            this.holdingGun = (bool)stream.ReceiveNext();
        }
    }
}
