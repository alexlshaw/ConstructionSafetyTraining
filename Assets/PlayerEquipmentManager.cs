using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    public GameObject helmetLocation;
    public GameObject glassesLocation;
    public GameObject maskLocation;
    public GameObject vestLocation;
    public GameObject leftFootLocation;
    public GameObject rightFootLocation;

    // Update is called once per frame
    void Update()
    {
        helmetLocation.SetActive(Globals.hasHat);
        glassesLocation.SetActive(Globals.hasGlasses);
        maskLocation.SetActive(Globals.hasMask);
        vestLocation.SetActive(Globals.hasVest);
        leftFootLocation.SetActive(Globals.hasBoots);
        rightFootLocation.SetActive(Globals.hasBoots);

    }
}
