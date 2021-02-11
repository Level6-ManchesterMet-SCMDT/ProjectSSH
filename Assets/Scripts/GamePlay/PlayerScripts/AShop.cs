using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AShop : MonoBehaviour
{
    public GameObject AbilityShop;
    public GameObject Gun;
    public GameObject Player;
    [SerializeField] GameObject zoom;
    [SerializeField] GameObject highlight;
    [SerializeField] GameObject sightRare;
    [SerializeField] GameObject hound;
    [SerializeField] GameObject gunpowderSmell;
    [SerializeField] GameObject pinpointSmell;
    [SerializeField] GameObject stepHearing;
    [SerializeField] GameObject radar;
    [SerializeField] GameObject sonar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        zoom.GetComponent<Button>().onClick.AddListener(Zoom);
        highlight.GetComponent<Button>().onClick.AddListener(Highlight);
        //sightRare.GetComponent<Button>().onClick.AddListener();
        //hound.GetComponent<Button>().onClick.AddListener(Hound);
        //gunpowderSmell.GetComponent<Button>().onClick.AddListener(GunpowderSmell);
        //pinpointSmell.GetComponent<Button>().onClick.AddListener(PinpointSmell);
        //stepHearing.GetComponent<Button>().onClick.AddListener(StepHearing);
        //radar.GetComponent<Button>().onClick.AddListener(Radar);
        //sonar.GetComponent<Button>().onClick.AddListener(Sonar);

    }

    void Control()
    {
        if (AbilityShop.activeInHierarchy)
        {
            Gun.GetComponent<Gun>().enabled = false;
            //Player.GetComponent<FirstPersonController>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Gun.GetComponent<Gun>().enabled = true;
        }
    }
    void Zoom()
    {
        //link to zoom ability
    }
    void Highlight()
    {
        //link to highlight ability
    }
}
