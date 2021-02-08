using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCount : MonoBehaviour
{
    public int ammo;
    public int maxAmmo;
    public Text ammoDisplay;
    public Text maxAmmoDisplay;
    // Update is called once per frame

    void Update()
    {
        ammoDisplay.text = ammo.ToString();
        maxAmmoDisplay.text = "/" + maxAmmo.ToString();
    }
}
