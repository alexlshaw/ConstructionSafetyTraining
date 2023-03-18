using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    public GameObject helmetLocation;
    public GameObject glassesLocation;
    public GameObject maskLocation;
    public GameObject vestLocation;
    public GameObject leftFootLocation;
    public GameObject rightFootLocation;
    public GameObject harnessLocation;

    public GameObject rope;

    private void Start()
    {
        updateEquipment();
        GameEvents.current.onUpdateEquipment += updateEquipment;
    }
    public void updateEquipment()
    {
        helmetLocation.SetActive(Globals.hasHat);
        glassesLocation.SetActive(Globals.hasGlasses);
        maskLocation.SetActive(Globals.hasMask);
        vestLocation.SetActive(Globals.hasVest);
        leftFootLocation.SetActive(Globals.hasBoots);
        rightFootLocation.SetActive(Globals.hasBoots);
        harnessLocation.SetActive(Globals.hasHarness);
    }


}
