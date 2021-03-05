using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Animations.Rigging;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    [Header("Movement")]

    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool FinishedJumping = false;
    bool isGrounded;
    bool grounded;

    [Header("Gun")]

    [SerializeField] GameObject gun;
    [SerializeField] Camera fpsCam;
    public float mouseSensitivity;
    public int ZoomPower = 0;
    public static int ZoomScale = 0;
    bool Armed = true;
    bool holdingGun = true;

    [Header("Looking")]

    [SerializeField] Camera Cam;
    [SerializeField] GameObject cameraHolder;
    float verticalLookRotation;

    [Header("Rig")]

    [SerializeField] GameObject rog_layers_hand_IK;
    [SerializeField] GameObject upperBody;
    [SerializeField] GameObject arms;
    Rig constrainthands;
    RigBuilder rigBuilder;
    //TwoBoneIKConstraint constraintRightHand;
    //TwoBoneIKConstraint constraintLeftHand;
    //RigBuilder rb;

    [Header("Other")]

    [SerializeField] Animator animator;
    [SerializeField] GameObject canvas;
    public static GameObject LocalPlayerInstance;
    public bool keyboardEnabled = true;
    public bool shopActive = false;
    public CharacterController player;
    private bool dead;
    Vector3 velocity;


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
        constrainthands = rog_layers_hand_IK.GetComponent<Rig>();

        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            Destroy(Cam.gameObject);

            for (int i = 0; i < canvas.transform.GetChild(0).transform.childCount; i++)
            {
                canvas.transform.GetChild(0).transform.GetChild(i).transform.gameObject.SetActive(false);
            }

            fpsCam.enabled = false;
            arms.layer = 0;
            gun.layer = 0;

            for (int i = 0; i < gun.transform.childCount; i++)
            {
                gun.transform.GetChild(i).gameObject.layer = 0;

                for (int j = 0; j < gun.transform.GetChild(i).transform.childCount; j++)
                {
                    gun.transform.GetChild(i).transform.GetChild(j).gameObject.layer = 0;
                }
            }
        }else
        {
            canvas.SetActive(true);
        }
    }


    void Update()
    {
        if(photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            Gravity();
            if (keyboardEnabled)
            {
                Look();
                Move();
                Jump();
                Rifle();
                UI();
                Shop();
            }
        }

        if (holdingGun && !dead)
        {
            constrainthands.weight = 1.0f;
        }
        else
            constrainthands.weight = 0.0f;
    }

    void Shop()
    {
        if (Input.GetKeyDown("b") && shopActive)
        {
            canvas.transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
            shopActive = false;
            gun.GetComponent<Gun>().enabled = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown("b") && !shopActive)
        {
            canvas.transform.GetChild(0).GetChild(6).gameObject.SetActive(true);
            shopActive = true;
            gun.GetComponent<Gun>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
    }

    public static void UpdateZoom (int ZoomPower)
    {
        ZoomScale = ZoomPower;
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

        if (Input.GetKey(KeyCode.Mouse1) && Armed && !animator.GetBool("Running"))
        {
            animator.SetBool("Aiming", true);

            if (ZoomScale == 0) 
            {
                if (Cam.fieldOfView >= 70)
                {
                    Gun.GetSpread(0.040f); //change accuracy when scoped in
                    mouseSensitivity = 2.7f;
                    Cam.fieldOfView -= 0.7f;
                }
            }
            else if (ZoomScale == 1)
            {

                if (Cam.fieldOfView >= 50)
                {
                    Gun.GetSpread(0.020f);
                    mouseSensitivity = 2.4f;
                    Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        Cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 50;
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") < 0 && Cam.fieldOfView <= 80)
                    {
                        Cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 50;
                    }
                }
            }
            else if (ZoomScale == 2)
            {
                if (Cam.fieldOfView >= 30)
                {
                    Gun.GetSpread(0.01f);
                    mouseSensitivity = 2.1f;
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        Cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 80;
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") < 0 && Cam.fieldOfView <= 80)
                    {
                        Cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 80;
                    }
                }
            }
            else if (ZoomScale == 3)
            {
                if (Cam.fieldOfView >= 10)
                {
                    Gun.GetSpread(0.006f);
                    mouseSensitivity = 0.5f;
                    if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        Cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 100;
                    }
                    if (Input.GetAxis("Mouse ScrollWheel") < 0 && Cam.fieldOfView <= 80)
                    {
                        Cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 100;
                    }
                }
            }
            
        }
        else
        {
            animator.SetBool("Aiming", false);
            if (Cam.fieldOfView <= 80)
            {
                mouseSensitivity = 3.0f;
                Cam.fieldOfView += 0.7f;
            }
        }

        if (Input.GetMouseButtonDown(0) && !animator.GetBool("Running"))
        {
            animator.SetBool("Shooting", true);
        }
        else if (Input.GetMouseButtonUp(0)) { animator.SetBool("Shooting", false); }
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
        Debug.Log(move);
        if (Input.GetKey(KeyCode.LeftShift) && move != new Vector3(0,0,0))
        {
            player.Move(move * Time.deltaTime * 4);
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }
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

    void Look()
    {
        if (!shopActive && (Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0))
        {
            transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

            verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -80f, 80f);

            cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
        }

    }

    void UI()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            canvas.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
        }else canvas.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
    }

    public void RespawnPlayer(string deathAnim)
    {
        dead = true;
        StartCoroutine(respawnWait(deathAnim));
    }

    IEnumerator respawnWait(string deathAnim)
    {
        //Wait for 4 seconds
        player.GetComponent<CharacterController>().enabled = false;
        yield return new WaitForSeconds(5);
        this.GetComponent<PlayerStats>().Spawned();
        animator.SetBool(deathAnim, false);
        Transform spawnpoint = SpawnManager.Instance.GetSpawnPoint();
        player.transform.position = spawnpoint.position;
        player.GetComponent<CharacterController>().enabled = true;
        dead = false;
    }

    //Animations

    void FinishedJump()
    {
        FinishedJumping = true;
    }

    void FinishedPuttingBack()
    {
        Armed = false;
        gun.SetActive(false);
        SetHoldingGunState(false);
    }

    void StartEquipping()
    {
        SetHoldingGunState(true);
        gun.SetActive(true);
    }

    void FinishedEquipping()
    {
        Armed = true;
        SetHoldingGunState(true);
    }

    public void SetGroundedState(bool _grounded)
    {
        grounded = _grounded;
    }

    public void SetHoldingGunState(bool _holdingGun)
    {
        holdingGun = _holdingGun;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(holdingGun);
        }
        else
        {
            this.holdingGun = (bool)stream.ReceiveNext();
        }
    }

   
}
