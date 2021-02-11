using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    

    public GameObject shopUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("b") && !shopUI.activeInHierarchy)
            shopUI.SetActive(true);

        if (Input.GetKey("b") && shopUI.activeInHierarchy)
            shopUI.SetActive(false);
    }
    void OpenShop()
    {
       
    }
}
