using UnityEngine;

public class ModelEquipmentManager : MonoBehaviour
{
    GameObject player;
    PlayerEquipmentManager playerEquipmentManager;
    MenuController menuController;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerEquipmentManager = player.GetComponent<PlayerEquipmentManager>();
        menuController = player.GetComponentInChildren<MenuController>();
    }

    public void EquipHelmet()
    {
        Globals.hasHat = true;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }
    public void UnequipHelmet()
    {
        Globals.hasHat = false;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }

    public void EquipGlasses()
    {
        Globals.hasGlasses = true;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }
    public void UnequipGlasses()
    {
        Globals.hasGlasses = false;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }

    public void EquipMask()
    {
        Globals.hasMask = true;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }
    public void UnequipMask()
    {
        Globals.hasMask = false;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }

    public void EquipVest()
    {
        Globals.hasVest = true;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }
    public void UnequipVest()
    {
        Globals.hasVest = false;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }

    public void EquipGloves()
    {
        Globals.hasGloves = true;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }
    public void UnequipGloves()
    {
        Globals.hasGloves = false;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }

    public void EquipBoots()
    {
        Globals.hasBoots = true;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }
    public void UnequipBoots()
    {
        Globals.hasBoots = false;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }

    public void EquipHarness()
    {
        Globals.hasHarness = true;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }
    public void UnequipHarness()
    {
        Globals.hasHarness = false;
        playerEquipmentManager.updateEquipment();
        menuController.updateToggles();

    }

}
